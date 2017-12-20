using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexHandler : MonoBehaviour {
    public int x_, y_,localplayer_;
    bool selected_;
    public static Color LightBlue;
    Color Default_;
    private Color Darken(Color C)
    {
        return Color.Lerp(C, Color.black, 0.5f);
    }
    private void COLOR(Color C)
    {
        GetComponent<SpriteRenderer>().color = C;
    }
    
    private void OnMouseEnter()
    {
        if (selected_)
            COLOR(Darken(LightBlue));
        else
            COLOR(Darken(Default_));
    }
    private void OnMouseExit()
    {
        if (selected_)
            COLOR(LightBlue);
        else
            COLOR(Default_);
    }
    public void Unclick()
    {
        selected_ = false;
        COLOR(Default_);
    }
    public void Click()
    {
        selected_ = true;
        COLOR(LightBlue);
    }
    /*bool GuiOn;
    public void OnGUI()
    {
        string abduli = "ghoorbaghe";
        if (GuiOn)
        {//check if gui should be on. If false, the gui is off, if true, the gui is on
         // Make a background box
            GUI.Box(new Rect(10, 10, 100, 90), "You sure?");
            // Make the first button. If pressed, quit game 
            abduli = GUI.TextField(new Rect(20, 40, 80, 20), abduli,25,new GUIStyle());

            // Make the second button.If pressed, sets the var to false so that gui disappear
            if (GUI.Button(new Rect(20, 70, 80, 20), "No"))
            {
                Debug.Log(abduli);
            }
        }
        GuiOn = true;
    }*/
    private void OnMouseUpAsButton()
    {
        
        if (selected_)
            GetComponentInParent<GameBoardHandler>().HexUnclicked(this);
        else
            GetComponentInParent<GameBoardHandler>().HexClicked(this);
    }
    // Use this for initialization
    public PairHex Pair()
    {
        return new PairHex(x_, y_);
    }
    public void Set(int x,int y,int player,Vector3 pos)
    {
        x_ = x;
        y_ = y;
        gameObject.transform.position = pos;
        localplayer_ = player;
        if (player == 0)
            Default_ = Color.white;
        else
            Default_ = Color.Lerp(Color.white, Color.black, 0.2f);
    }
    void Start () {
        if (selected_)
            Click();
        else
            Unclick();
	}
}
