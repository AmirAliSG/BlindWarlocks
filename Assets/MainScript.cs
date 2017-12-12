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
    
    [UnityEngine.Serialization.FormerlySerializedAsAttribute("cards")]
    
    
    public Player AmirAli;
    public static MainScript instance_;

    [MenuItem("fucker/Change #&b")]
    static void hapalan()
    {
        int Canvas_Height = 502;
        int ismagic = 1;
        int borderdistance = Canvas_Height/20;
        string a = Selection.activeGameObject.name;
        //GameObject.Find("Canvas").GetComponent<RectTransform>.
        int Radius = Canvas_Height/(2*4);
        if (a[0] == 'M')
            ismagic += 2;
        RectTransform ActiveRectTransform = Selection.activeGameObject.GetComponent<RectTransform>();
        ActiveRectTransform.anchorMax = new Vector2(0.115f,1f - (float)(Radius + borderdistance )* ismagic/Canvas_Height);
        ActiveRectTransform.anchorMin = new Vector2(0.115f,1f - (float)(Radius + borderdistance) * ismagic/Canvas_Height);
        //Selection.activeGameObject.transform.position += Vector3.forward * 10;
        
        int angel = ((int)a[a.Length - 2] - (int)'0') * 60;


        Vector3 Direction = new Vector3(Mathf.Cos((angel - 90) * Mathf.PI / 180), Mathf.Sin((angel - 90) * Mathf.PI / 180), 0);
        if (a[a.Length - 2] == '7')
        {
            Direction = new Vector3(0, 0, 0);
            angel = 0;
        }
        else
            Selection.activeGameObject.transform.rotation = Quaternion.Euler(0, 0, angel+180);
        ActiveRectTransform.anchoredPosition = Radius * Direction;
        
    }
    
    private Canvas canvas;
    
    private List<Pair<CardData,CardVisualizer>> hand_;
    
    // Use this for initialization
    void Start() {
        instance_ = this;
        canvas = FindObjectOfType<Canvas>();
        AmirAli = new Player();
        
    }
    public void SelectDirectionShower(DirectionShowerHandler direction)
    {
        direction.selected = true;
        if (direction.ifmagic_)
        {
            if (AmirAli.selected_magic_direction_ != null)
                AmirAli.selected_magic_direction_.selected = false;
            if (AmirAli.selected_magic_direction_ == direction)
                AmirAli.selected_magic_direction_ = null;
            else
                AmirAli.selected_magic_direction_ = direction;
        }
        else
        {
            if (AmirAli.selected_move_direction_ != null)
                AmirAli.selected_move_direction_.selected = false;
            if (AmirAli.selected_move_direction_ == direction)
                AmirAli.selected_move_direction_ = null;
            else
                AmirAli.selected_move_direction_ = direction;
        }
        AmirAli.CheckCanReady();
        gameObject.GetComponent<UnityEngine.UI.Selectable>().Select();
    }
    public void SelectCardFromHand(CardData data, CardVisualizer card)
    {
        AmirAli.SelectCard(data, card);
        if (data.IsDefault == false)
            CardDashboardHandler.instance_.Remove(data, card);
    }
    public void UnselectCard(CardData data,CardVisualizer cdvis)
    {
        if (data.IsDefault == false)
            CardDashboardHandler.instance_.Draw(data);
        AmirAli.UnselectCard();///this is required when only amirali have the not ishandmode_ shits
    }
}
