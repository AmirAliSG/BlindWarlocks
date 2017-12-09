using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardVisualizer : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    [SerializeField]
    private Image background_;
    public Text name_;
    private bool handmode_;
    private CardData data_;
	public void Visualize(CardData data,bool handmode)
    {
        data_ = data;
        background_.sprite = data.Img;
        name_.text = data.name;
        handmode_ = handmode;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(handmode_)
        Debug.Log("Fuck Amirali");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Really Fuck Amirali");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Debug.Log("Dude, Fuck Amirali FFS");
    }
}
