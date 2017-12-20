using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
//check all gameobject.find() names
[System.Serializable]
public class CardData {
    public Sprite Img;
    public string name;
    public int level;
    public bool IsDefault;
    public bool IsActive;
    public bool IsSuicidal;
}
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
    public static float movespeed_ = 1f;
    public PairHex place_;//not sure
    public GameObject figure_;
    private Vector3 ToGo_;
    public void SetGoing(Vector3 ToGo)
    {
        ToGo_ = ToGo;
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
        figure_.transform.Translate(moveunit * (destination - figure_.transform.position));
        return false;
    }
    public void COLOR(Color col)
    {
        figure_.GetComponent<MeshRenderer>().material.color = col;//should be tested
    }
    public Warlock(GameObject figure,Color color)
    {
        figure_ = figure;
        figure_.transform.position = Vector3.zero;
        COLOR(color);
        figure_.transform.parent = GameObject.Find("GameBoard").transform;
    }
}
[System.Serializable]
public class Player
{
    static public int MAXPLAYERS = 6;
    //static for now
    public string name_;
    public int number_;
    public Color color_;
    public Warlock warlock_;
    public CardVisualizer show_played_card_;
    public int hand_size_;
    public int score_;
    public CardData[] active_cards_;//
    public GameObject player_panel_;
    
    ////this player:
    static public List<CardData> hand_;//
    static public CardData selected_card_;
    static public CardVisualizer show_selected_card_;
    static public DirectionShowerHandler selected_magic_direction_;
    static public DirectionShowerHandler selected_move_direction_;
    static public bool isready_;
    static public bool canready_;

    static public void CheckCanReady()
    {
        Button B = GameObject.Find("ReadyButton").GetComponent<Button>();
        if (selected_magic_direction_ != null && selected_move_direction_ != null && selected_card_ != null)
            B.interactable = true;
        else
            B.interactable = false;
    }
    static public void SelectCard(CardData data, CardVisualizer cdref)
    {
        if (show_selected_card_ != null)
            MainScript.instance_.UnselectCard(selected_card_, show_selected_card_);
        show_selected_card_ = Object.Instantiate(GameObject.Find("CardDashboard").GetComponent<CardDashboardHandler>().card_prefab_, new Vector3(310, 280, 0), new Quaternion(0, 0, 0, 0), GameObject.Find("Canvas").transform).GetComponent<CardVisualizer>();
        show_selected_card_.Visualize(data, false);
        selected_card_ = data;
        CheckCanReady();
    }
    static public void UnselectCard()
    {
        selected_card_ = null;
        GameObject.Destroy(show_selected_card_.gameObject);
        CheckCanReady();
    }
    static public void SelectDirectionShower(DirectionShowerHandler direction)
    {
        direction.selected = true;
        if (direction.ifmagic_)
        {
            if (selected_magic_direction_ != null)
                selected_magic_direction_.selected = false;
            if (selected_magic_direction_ == direction)
                selected_magic_direction_ = null;
            else
                selected_magic_direction_ = direction;
        }
        else
        {
            if (selected_move_direction_ != null)
                selected_move_direction_.selected = false;
            if (selected_move_direction_ == direction)
                selected_move_direction_ = null;
            else
                selected_move_direction_ = direction;
        }
        CheckCanReady();
    }
    ////

    public Player(string name,int number, GameObject panel,Warlock warlock)
    {
        name_ = name;
        number_ = number;
        warlock_ = warlock;
        score_ = 0;
        player_panel_ = panel;
        player_panel_.GetComponent<Text>().text = name_ + "(BLUE)\n" + score_.ToString();
        
    }
    public Player(string name, int number, GameObject panel, GameObject figure_prefab,Color color)
    {
        name_ = name;
        number_ = number;
        score_ = 0;
        player_panel_ = panel;
        player_panel_.GetComponent<Text>().text = name_ + "(BLUE)\n" + score_.ToString();
        warlock_ = new Warlock(GameObject.Instantiate(figure_prefab, player_panel_.transform), Color.blue);
        
    }
}

public class Pair<T1, T2>
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
}

public class MainScript : MonoBehaviour {
    //[UnityEngine.Serialization.FormerlySerializedAsAttribute("cards")]
    
    public static MainScript instance_;
    public GameObject Figure_Prefab_;
    public GameObject Panel_Prefab_;
    public GameObject Direction_Prefab_;
    public GameObject[] DirectionShowers_;
    public Player[] All_;
    public bool figuresmoveonebyone;
    public bool movingphase_;
    public int my_number_;/////

    private void Update()
    {
        if (movingphase_)
        {
            if (figuresmoveonebyone)
            {
                int i;
                for (i = 0; i < All_.Length && All_[i].warlock_.Move(); i++) ;
                if (i == All_.Length)
                    MovePhaseEnded();
            }
            else
            {
                bool reached = true;
                for (int i = 0; i < All_.Length; i++)
                    reached = reached & All_[i].warlock_.Move();
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
    void Start() {
        instance_ = this;
        InstantiateDirectionShowers();
        All_ = new Player[3];
        for(int i = 0; i < 3; i++)
        {
            All_[i] = new Player("Ali"+i.ToString(),i, GameObject.Instantiate(Panel_Prefab_,new Vector3(800,250+3*i),new Quaternion(0,0,0,0), GetComponentInChildren<Canvas>().transform), Figure_Prefab_,Color.blue);
        }
        All_[0].warlock_.SetGoing(new Vector3(3, 2, 0));
        All_[1].warlock_.SetGoing(new Vector3(2, 3, 0));
        All_[2].warlock_.SetGoing(new Vector3(1, 2, 0));
        GetComponentInChildren<Scrollbar>().onValueChanged.AddListener(ScrollChanged);


        StartCoroutine(GetText());
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
        UnityWebRequest www = UnityWebRequest.Get("file:///C:/Users/AmirAli/Documents/BlindWarlocks/Assets/ghootisoorati.txt");
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
        www = UnityWebRequest.Get("file:///C:/Users/AmirAli/Documents/BlindWarlocks/Assets/maps.txt");
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
    public void InstantiateDirectionShowers()
    {
        DirectionShowers_ = new GameObject[14];
        for(int i = 1; i < 14; i++)
        {
            DirectionShowers_[i] = GameObject.Instantiate(Direction_Prefab_, GetComponentInChildren<Canvas>().transform);
            DirectionShowers_[i].GetComponent<DirectionShowerHandler>().Set(i);
        }
    }

    public void ReadyButtonClicked()
    {
        //sendinfo
        GameObject.Find("ReadyButton").GetComponent<Button>().interactable = false;//Player.CheckCanReady
        movingphase_ = true;
    }
    public void ScrollChanged(float value)
    {
        GetComponentInChildren<GameBoardHandler>().transform.rotation = Quaternion.Euler(value * -90, 0, 90);
    }
    public void SelectDirectionShower(DirectionShowerHandler direction)
    {
        Player.SelectDirectionShower(direction);
        GameObject.Find("TEXT").GetComponent<UnityEngine.UI.Selectable>().Select();
    }
    public void SelectCardFromHand(CardData data, CardVisualizer card)
    {
        Player.SelectCard(data, card);
        if (data.IsDefault == false)
            CardDashboardHandler.instance_.Remove(data, card);
    }
    public void UnselectCard(CardData data,CardVisualizer cdvis)
    {
        if (data.IsDefault == false)
            CardDashboardHandler.instance_.Draw(data);
        Player.UnselectCard();///this is required when only thisPlayer have the not ishandmode_ shits
    }
    
}
//gamescale fullscreen
//network
//gameboard
//rules
//other scene:menu, option

// network, rules, game screen, optional animation