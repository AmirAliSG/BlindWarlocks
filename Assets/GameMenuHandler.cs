using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMenuHandler : MonoBehaviour {
    public GameObject MenuButton_;
    public GameObject Name_Input_;
    public GameObject ColorDropDown_;
    public List<GameObject> Buttons;
    public GameObject CreateButton(string name)
    {
        GameObject NewButton = GameObject.Instantiate(MenuButton_, transform);
        NewButton.name = name;
        NewButton.GetComponentInChildren<UnityEngine.UI.Text>().text = name;
        return NewButton;
    }
    private void Start()
    {
        isit = true;
        Buttons = new List<GameObject>();
    }
    public void InstantiateMode(string mode)
    {
        ColorDropDown_.SetActive(false);
        Name_Input_.SetActive(false);
        for (int i = 0; i < Buttons.Count; i++)
        {
            if(Buttons[i].activeInHierarchy)
                DestroyImmediate(Buttons[i]);
        }
        Buttons = new List<GameObject>();

       /* if (mode == "PauseGame")
        {
            Buttons.Add(CreateButton("Resume"));
        }
        if (mode == "StartGame")
        {
            Buttons.Add(CreateButton("Create"));
        }
        if (mode == "StartGame")
        {
            Buttons.Add(CreateButton("Join"));
        }
        if (mode == "StartGame")
        {
            ColorDropDown_.SetActive(true);
            Buttons.Add(ColorDropDown_);
        }
            
        if (mode == "StartGame")
        {
            Name_Input_.SetActive(true);
            Buttons.Add(Name_Input_);
        }
            
        if (mode == "StartGame" || mode == "PauseGame")
        {
            Buttons.Add(CreateButton("Rules"));
        }
        if (mode == "StartGame" || mode == "PauseGame")
        {
            Buttons.Add(CreateButton("Options"));
        }
        if (mode == "PauseGame")
        {
            Buttons.Add(CreateButton("Leave Game"));
        }
        if (mode == "StartGame")
        {
            Buttons.Add(CreateButton("Map Editor"));
        }*/

        if (mode == "StartGame" || mode == "PauseGame")
        {
            Buttons.Add(CreateButton("Exit"));
        }
        for (int i = 0; i < Buttons.Count; i++)
        {
            Buttons[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(0, -50 * (i+1), 0);
        }
    }
    bool isit;
    public void TEST()
    {
        if (isit)
            InstantiateMode("StartGame");
        else
            InstantiateMode("PauseGame");
        isit = !isit;
    }
}
