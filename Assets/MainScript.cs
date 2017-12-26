using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
//check all gameobject.find() names

public class Hand
{
    public List<CardData> cards_;
    /*public Hand()
    {
        add default cards
    }*/
}
public class Warlock
{
    //Move(place_.Next(movedirection_).PositionOf())
    public static float movespeed_ = 2f;
    public PairHex place_;//not sure
    public GameObject figure_;
    private Vector3 ToGo_;
    public void SetGoing(Vector3 ToGo,PairHex place)
    {
        ToGo_ = ToGo;
        place_ = place;
    }
    public bool Move()
    {
        Vector3 destination = ToGo_;
        float moveunit = movespeed_ * Time.deltaTime;
        if (Vector3.Distance(figure_.transform.position, destination) < moveunit)
        {
            figure_.transform.position = destination;
            return true;
        }
        figure_.transform.position += (moveunit * (destination - figure_.transform.position));//you want shit got real ? change "position +=" to "translate"
        return false;
    }
    public void COLOR(Color col)
    {
        figure_.GetComponent<MeshRenderer>().material.color = col;//should be tested
    }

    public void Kill(GameObject player_panel_)
    {
        figure_.transform.parent = player_panel_.transform;
        //Debug.Log(warlock_.figure_.transform.position.ToString());
        figure_.transform.localPosition = new Vector3(0, 0, 0);
        figure_.transform.rotation = Quaternion.Euler(-90, -180, 0);
        //Debug.Log(warlock_.figure_.transform.position.ToString());
        //warlock_.figure_.transform.localScale *= 10;
    }
    public void ReviveAt(GameObject GameBoard, PairHex Place)
    {
        figure_.transform.parent = GameBoard.transform;
        figure_.transform.position = GameBoard.GetComponent<GameBoardHandler>().Get(Place).transform.position;
        figure_.transform.localRotation = Quaternion.Euler(0, 0, -90);
    }
    public Warlock(GameObject figure,Color color)
    {
        figure_ = figure;
        figure_.transform.position = Vector3.zero;
        COLOR(color);
        //figure_.transform.parent = GameObject.Find("GameBoard").transform;
    }
}
[System.Serializable]
public class Player
{
    static public int MAXPLAYERS = 6;
    //static for now
    public string name_;
    public int number_, hand_size_, score_;
    public Color color_;
    public Warlock warlock_;
    public CardVisualizer show_played_card_;
    public CardData[] active_cards_;//
    public GameObject player_panel_;//need panel or just text ?
    public Direction magic_direction_,move_direction_;
    ////this player:
    static public List<CardData> hand_;//
    static public CardData selected_card_;
    static public DirectionShowerHandler selected_magic_direction_;
    static public DirectionShowerHandler selected_move_direction_;
    static public bool isready_;
    static public bool canready_;

    
    public void SetDirections(Direction move,Direction magic)
    {
        magic_direction_ = magic;
        move_direction_ = move;
        string S = player_panel_.GetComponent<Text>().text;
        player_panel_.GetComponent<Text>().text = S.Substring(0, S.IndexOf("Move Direction: ") + "Move Direction: ".Length) + move_direction_.ToString() + S.Substring(S.IndexOf("Move Direction: ") + "Move Direction: ".Length);

            ///magic
    }
    public void SelectCard(CardData data, CardVisualizer cdref)
    {
        if (show_played_card_ != null)
            MainScript.instance_.UnselectCard(selected_card_, show_played_card_);

        show_played_card_ = Object.Instantiate(GameObject.Find("CardDashboard").GetComponent<CardDashboardHandler>().card_prefab_, new Vector3(0, 0, 0), new Quaternion(0, 0, 0, 0), player_panel_.transform).GetComponent<CardVisualizer>();
        show_played_card_.transform.localPosition = Vector3.zero;
        show_played_card_.transform.localScale *= 0.5f;
        show_played_card_.name = number_.ToString();
        show_played_card_.Visualize(data, false);
        selected_card_ = data;
        CheckCanReady();
    }
    public void UnselectCard()
    {
        selected_card_ = null;
        GameObject.Destroy(show_played_card_.gameObject);
        CheckCanReady();
    }

    static public void SelectDirectionShower(DirectionShowerHandler direction,Player me)
    {
        direction.selected = true;
        if (direction.ifmagic_)
        {
            if (selected_magic_direction_ != null)
                selected_magic_direction_.selected = false;
            if (selected_magic_direction_ == direction)
            {
                selected_magic_direction_ = null;
                me.magic_direction_ = null;
            }
            else
            {
                selected_magic_direction_ = direction;
                me.magic_direction_ = direction.direction_;
            }
        }
        else
        {
            if (selected_move_direction_ != null)
                selected_move_direction_.selected = false;
            if (selected_move_direction_ == direction)
            {
                selected_move_direction_ = null;
                me.move_direction_ = null;
            }
            else
            {
                selected_move_direction_ = direction;
                me.move_direction_ = direction.direction_;
            }
        }
        CheckCanReady();
    }
    static public void CheckCanReady()
    {
        Button B = GameObject.Find("ReadyButton").GetComponent<Button>();
        if (selected_magic_direction_ != null && selected_move_direction_ != null && selected_card_ != null)
            B.interactable = true;
        else
            B.interactable = false;
    }
    ////

    public Player(string name, int number, GameObject panel, GameObject figure_prefab,Color color)
    {
        name_ = name;
        number_ = number;
        score_ = 0;
        player_panel_ = panel;
        player_panel_.GetComponent<Text>().text = name_ + "("+ score_.ToString() + ")\nMove Direction: \nMagic Direction: \n";
        warlock_ = new Warlock(GameObject.Instantiate(figure_prefab), color);
        panel.GetComponent<RectTransform>().position = new Vector3(-5, 0,0);
        panel.GetComponentInChildren<SpriteRenderer>().color = color;
        warlock_.Kill(player_panel_);
        player_panel_.transform.Translate(0, 2*number_, 0);
    }
}

/*public class Pair<T1, T2>
{
    private T1 item1_;
    public T1 pItem1
    {
        get
        {
            return item1_;
        }
        set
        {
            item1_ = value;
        }
    }
    public T2 pItem2 { get; set; }

    public Pair(T1 item1,T2 item2)
    {
        item1_ = item1;
        pItem2 = item2;
    }
}*/

public class MainScript : MonoBehaviour {
    //[UnityEngine.Serialization.FormerlySerializedAsAttribute("cards")]
    
    public static MainScript instance_;
    public GameObject Main_Canvas_;
    public GameObject PlayersPanels_;
    public GameObject Figure_Prefab_;
    public GameObject Panel_Prefab_;
    public GameObject Direction_Prefab_;
    public GameObject[] DirectionShowers_;
    public GameObject Panel_;
    public GameObject GameMenu_;
    public Player[] All_Players_;
    public bool figuresmoveonebyone;
    public bool movingphase_;
    private bool IsSelectMode_;
    public int my_number_;/////

    public GameBoardHandler FindGameBoard()
    {
        return GetComponentInChildren<GameBoardHandler>();
    }
    public GameObject FindCanvas()
    {
        if(Main_Canvas_ == null)
            Main_Canvas_ = GameObject.Find("MainCanvas");
        return Main_Canvas_;
    }
    public GameObject FindPanel()
    {
        if (Panel_ == null)
            Panel_ = GetComponentInChildren<GameMenuHandler>().gameObject;
        return Panel_;
    }
    private void Update()
    {
        if (movingphase_)
        {
            if (figuresmoveonebyone)
            {
                int i;
                for (i = 0; i < All_Players_.Length && All_Players_[i].warlock_.Move(); i++) ;
                if (i == All_Players_.Length)
                    MovePhaseEnded();
            }
            else
            {
                bool reached = true;
                for (int i = 0; i < All_Players_.Length; i++)
                    reached = reached & All_Players_[i].warlock_.Move();
                if (reached)
                    MovePhaseEnded();
            }
        }
    }
    void MovePhaseEnded()
    {
        movingphase_ = false;
        //
    }
    public void TakeInitialDataFromServer()
    {
        int playerscount = 4;
        All_Players_ = new Player[playerscount];//0 is me
        for (int i = 0; i < All_Players_.Length; i++)
        {
            All_Players_[i] = new Player("Ali" + i.ToString(), i, GameObject.Instantiate(Panel_Prefab_, new Vector3(50, 0), new Quaternion(0, 0, 0, 0), PlayersPanels_.transform), Figure_Prefab_, Color.blue);
        }
    }
    void Start() {
        instance_ = this;
        IsSelectMode_ = true;

        InstantiateDirectionShowers();
        TakeInitialDataFromServer();
        GetComponentInChildren<Scrollbar>().onValueChanged.AddListener(ScrollChanged);
        FindGameBoard().transform.rotation = Quaternion.Euler(0, 0, 90);
        FindPanel().SetActive(true); FindPanel().GetComponent<GameMenuHandler>().InstantiateMode("PauseGame");

        //StartCoroutine(GetText());
        //StartCoroutine(Upload());
    }

    IEnumerator Upload()
    {
        UnityWebRequest www = UnityWebRequest.Put("file:///C:/Users/AmirAli/Documents/BlindWarlocks/Assets/ghootisoorati.txt", "gholi");
        yield return www.Send();

        if (www.isNetworkError)
        {
            Debug.Log(www.error);
        }
        else
        {
            Debug.Log("Form upload complete!");
        }
    }
    IEnumerator GetText()
    {
        UnityWebRequest www = UnityWebRequest.Get("file:///C:/Users/AmirAli/Documents/BlindWarlocks/ghootisoorati.txt");
        yield return www.Send();

        if (www.isNetworkError)
        {
            
            Debug.Log(www.error);
        }
        else
        {
            // Show results as text
            Debug.Log(www.downloadHandler.text);
        }
    }


    /// <summary>
    /// after getting info from server: (number of you, number of other players)
    /// create player for each player
    /// at each round, you tell server you are ready(move direction, spell direction, carddata)
    /// and server tells you your place, players places and carddata they played and are they dead and their scores and whether the game ends
    /// </summary>
    /// <param name="value"></param>
    public int GlobalToLocalNumber(int global)
    {
        if (my_number_ == 1)
            return global;
        else
            return (global + Player.MAXPLAYERS - my_number_) % Player.MAXPLAYERS + 1;
    }

    //UI Handling:
    public void SetActivityOfDirectionShowers()
    {
        //for(int direction = 1;direction<7;direction++)
            //DirectionShowerFinder(direction, false).SetActive(GetComponentInChildren<GameBoardHandler>().Available(All_[my_number_].warlock_.place_.Next(direction)));
    }////end turn
     //UI elements:
    bool isit = true;
    public void TEST(GameObject GameBoard)
    {
        Example();
        /*/EscapeToMenu(true);
        if (isit)
        {
            for (int i = 0; i < All_Players_.Length; i++)
                All_Players_[i].warlock_.ReviveAt(GameBoard, new PairHex(i, 0));
        }
        else
        {
            for (int i = 0; i < All_Players_.Length; i++)
                All_Players_[i].warlock_.SetGoing(GameBoard.GetComponent<GameBoardHandler>().Get(new PairHex(i, 1)).transform.position, new PairHex(i, 1));
            movingphase_ = true;
        }
        isit = !isit;*/
    }
    public void InstantiateDirectionShowers()
    {
        DirectionShowers_ = new GameObject[14];
        for(int i = 1; i < 14; i++)
        {
            DirectionShowers_[i] = GameObject.Instantiate(Direction_Prefab_, FindCanvas().transform);
            DirectionShowers_[i].GetComponent<DirectionShowerHandler>().Set(i);
        }
    }
    public void ChangeGameMode()
    {

    }
    public void ReadyButtonClicked()
    {
        //sendinfo
        GameObject.Find("ReadyButton").GetComponent<Button>().interactable = false;//Player.CheckCanReady
    }
    public void ScrollChanged(float value)
    {
        FindGameBoard().transform.rotation = Quaternion.Euler(value * -90, 0, 90);
    }
    public void MouseOnCardInPanel(CardData data,string Name)
    {
        if (Player.selected_magic_direction_ != null)
            for (int i = 0; i < data.Relative_Kill_zones.Length; i++)
            {
                HexHandler A = FindGameBoard().Get(data.Relative_Kill_zones[i].Rotate(Player.selected_magic_direction_.direction_) + All_Players_[0].warlock_.place_);//:(((
                if (A != null)
                    A.Click();
            }
    }
    public void MouseExitFromCardInPanel(CardData data, string Name)
    {
        if (Player.selected_magic_direction_ != null)
            for (int i = 0; i < data.Relative_Kill_zones.Length; i++)
            {
                HexHandler A = FindGameBoard().Get(data.Relative_Kill_zones[i].Rotate(Player.selected_magic_direction_.direction_) + All_Players_[0].warlock_.place_);//:(((
                if (A != null)
                    A.Unclick();
            }
    }
    public void SelectDirectionShower(DirectionShowerHandler direction)
    {
        Player.SelectDirectionShower(direction,All_Players_[0]);
        GameObject.Find("TEXT").GetComponent<UnityEngine.UI.Selectable>().Select();
    }
    public void SelectCardFromHand(CardData data, CardVisualizer card)
    {
        All_Players_[0].SelectCard(data, card);
        if (data.IsDefault == false)
            CardDashboardHandler.instance_.Remove(data, card);
    }
    public void UnselectCard(CardData data,CardVisualizer cdvis)
    {
        //havaset bashe key mishe zad
        MouseExitFromCardInPanel(data, cdvis.name);
        if (data.IsDefault == false)
            CardDashboardHandler.instance_.Draw(data);
        All_Players_[0].UnselectCard();///this is required when only thisPlayer have the not ishandmode_ shits
    }
    public void MenuButtonClicked(Text txt)
    {
        /*char[] B = new char[1];
        B[0] = ' ';
        Debug.Log("Menu" + txt.text.Split(B)[0] + "Clicked");
        //gameObject.SendMessage("Menu" + txt.text.Split(B)[0] + "Clicked");*/
        Example();
    }

    void ApplyDamage(float damage)
    {
        Debug.Log(damage.ToString());
    }
    void Example()
    {
        ApplyDamage(4f);
        Debug.Log(gameObject.ToString());
        gameObject.SendMessage("ApplyDamage", 5.0F);
    }

    //Menu
    public void EscapeToMenu(bool isescape)
    {
        //FindCanvas().SetActive(!isescape);
        //FindGameBoard().gameObject.SetActive(!isescape);
        Panel_.SetActive(isescape);
        if (isescape)
            FindPanel().GetComponent<GameMenuHandler>().InstantiateMode("PauseGame");
    }


    void MenuResumeClicked()
    {
        EscapeToMenu(false);
    }
    public void MenuCreateClicked()
    {
        FindPanel().GetComponent<GameMenuHandler>().InstantiateMode("CreatingGame");
    }
    public void MenuJoinClicked()
    {
        FindPanel().GetComponent<GameMenuHandler>().InstantiateMode("JoiningGame");
    }
    public void MenuRulesClicked()
    {
        //Rules
    }
    public void MenuOptionsClicked()
    {
        //Options
    }
    public void MenuLeaveClicked()
    {
        //send info to server
        //destroy objects
        FindPanel().GetComponent<GameMenuHandler>().InstantiateMode("StartGame");
    }
    public void MenuExitClicked()
    {
        //leave game
        //leave app
    }
}
//gamescale fullscreen
//network
//gameboard
//rules
//other scene:menu, option

// network, rules, game screen, optional animation