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
public class Player
{
    public string name_;
    public List<CardData> hand_;/// <summary>
    /// /
    /// </summary>
    public CardData selected_card_;
    public CardVisualizer show_selected_card_;
    public DirectionShowerHandler selected_magic_direction_;
    public DirectionShowerHandler selected_move_direction_;
    public bool isready_;
    public bool canready_;
    public void CheckCanReady()
    {
        Button B = GameObject.Find("ReadyButton").GetComponent<Button>();
        if (selected_magic_direction_ != null && selected_move_direction_ != null && selected_card_ != null)
            B.interactable = true;
        else
            B.interactable = false;
    }
    public void SelectCard(CardData data,CardVisualizer cdref)
    {
        if (show_selected_card_ != null)
            MainScript.instance_.UnselectCard(selected_card_, show_selected_card_);
        show_selected_card_ = Object.Instantiate(GameObject.Find("CardDashboard").GetComponent<CardDashboardHandler>().card_prefab_, new Vector3(310, 280, 0), new Quaternion(0, 0, 0, 0), GameObject.Find("Canvas").transform).GetComponent<CardVisualizer>();
        show_selected_card_.Visualize(data, false);
        selected_card_ = data;
        CheckCanReady();
    }
    public void UnselectCard()
    {
        selected_card_ = null;
        GameObject.Destroy(show_selected_card_.gameObject);
        CheckCanReady();
    }
    public void SelectDirectionShower(DirectionShowerHandler direction)
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
}
public class Tile
{
    public int x;
    public int y;
    public int z;
    //HexHandler hex_;

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
    public Player thisPlayer;
    public static MainScript instance_;

    void Start() {
        instance_ = this;
        thisPlayer = new Player();

    }
    public void SelectDirectionShower(DirectionShowerHandler direction)
    {
        Debug.Log("WATISHAPPENING");
        thisPlayer.SelectDirectionShower(direction);
        gameObject.GetComponent<UnityEngine.UI.Selectable>().Select();
    }
    public void SelectCardFromHand(CardData data, CardVisualizer card)
    {
        thisPlayer.SelectCard(data, card);
        if (data.IsDefault == false)
            CardDashboardHandler.instance_.Remove(data, card);
    }
    public void UnselectCard(CardData data,CardVisualizer cdvis)
    {
        if (data.IsDefault == false)
            CardDashboardHandler.instance_.Draw(data);
        thisPlayer.UnselectCard();///this is required when only thisPlayer have the not ishandmode_ shits
    }
    
}
//gamescale fullscreen
//network
//gameboard
//rules
//other scene:menu, option