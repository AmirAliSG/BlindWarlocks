using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
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
    }
}
public class Player
{
    public string name_;
    public int number_;
    public Color color_;
    public Warlock warlock_;
    public CardVisualizer show_played_card_;
    public int hand_size_;


    static public List<CardData> hand_;/// <summary>
                                /// /all can be static
                                /// </summary>
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


    public Player(string name,int number,Warlock warlock)
    {
        name_ = name;
        number_ = number;
        warlock_ = warlock;
    }
    public Player()
    {

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

    public Player[] All_;
    public bool figuresmoveonebyone;
    public bool movingphase_;
    public int my_number_;/////
    public GameObject DirectionShowerFinder(int direction, bool ismagic)
    {
        if (ismagic)
            return GameObject.Find("MagicDirectionShower (" + direction.ToString() + ")");
        else
            return GameObject.Find("DirectionShower (" + direction.ToString() + ")");
    }
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
        All_ = new Player[3];
        for(int i = 0; i < 3; i++)
        {
            All_[i] = new Player("Ali"+i.ToString(),i,new Warlock(GameObject.Instantiate(Figure_Prefab_,transform),Color.blue));
        }
        All_[0].warlock_.SetGoing(new Vector3(3, 2, 1));
        All_[1].warlock_.SetGoing(new Vector3(2, 3, 3));
        All_[2].warlock_.SetGoing(new Vector3(1, 2, 3));
        GetComponentInChildren<Scrollbar>().onValueChanged.AddListener(ScrollChanged);
        
    }/// <summary>
    /// after getting info from server:
    /// create player for each player
    /// 
    /// </summary>
    /// <param name="value"></param>
    public void ScrollChanged(float value)
    {
        GetComponentInChildren<GameBoardHandler>().transform.rotation = Quaternion.Euler(value * -90, 0, 90);
    }

    public void CheckDirectionAvailability(int direction)
    {
        //DirectionShowerFinder(direction,false).SetActive(GetComponentInChildren<GameBoardHandler>().Available(thisPlayer.place_.Next(direction), thisPlayer.number_));
    }////end turn
    
    public void ReadyButtonClicked()
    {
        //sendinfo
        GameObject.Find("ReadyButton").GetComponent<Button>().interactable = false;//Player.CheckCanReady
        movingphase_ = true;
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