using System;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DirectionShowerHandler : MonoBehaviour, IPointerClickHandler {
    const float Radius_Proportion = 12f;
    const int border_Proportion = 30;
    const float Bottom_offset = 150;
    public static void AnchorRight(GameObject gameObject,int direction,bool ismag)
    {
        float Canvas_Height = gameObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.height;
        float Canvas_Width = gameObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.width;
        int ismagic = 1;
        if (ismag)
            ismagic += 2;
        float Radius = Canvas_Height / Radius_Proportion;
        float borderdistance = Canvas_Height / border_Proportion;
        RectTransform ActiveRectTransform = gameObject.GetComponent<RectTransform>();
        ActiveRectTransform.anchorMax = new Vector2(1f - Radius * 3 / (2 * Canvas_Width), ( Bottom_offset + ((float)(Radius + borderdistance) * ismagic)) / Canvas_Height);
        ActiveRectTransform.anchorMin = new Vector2(1f - Radius * 3 / (2 * Canvas_Width), ( Bottom_offset + ((float)(Radius + borderdistance) * ismagic)) / Canvas_Height);
    }
    public static void MakeItRight(GameObject gameObject, int direction, bool ismag)
    {

        float Canvas_Height = gameObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.height;
        float Canvas_Width = gameObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.width;
        RectTransform ActiveRectTransform = gameObject.GetComponent<RectTransform>();
        int ismagic = 1;
        if (ismag)
            ismagic += 2;
        float borderdistance = Canvas_Height / border_Proportion;
        //GameObject.Find("Canvas").GetComponent<RectTransform>.
        float Radius = Canvas_Height / Radius_Proportion;


        //Selection.activeGameObject.transform.position += Vector3.forward * 10;

        float angel = direction * 60;


        Vector3 Direction = new Vector3(Mathf.Cos((angel - 90) * Mathf.PI / 180), Mathf.Sin((angel - 90) * Mathf.PI / 180), 0);
        if (direction == 7)
        {
            Direction = Vector3.zero;
            angel = -180;
            GameObject.DestroyImmediate(gameObject.GetComponent<Image>());
            Text Javad = gameObject.AddComponent<Text>();
            gameObject.GetComponent<Selectable>().targetGraphic = Javad;
            Javad.text = "STOP";
            Javad.fontStyle = FontStyle.Bold;
            
            Javad.alignment = TextAnchor.MiddleCenter;
            Javad.font = Font.CreateDynamicFontFromOSFont("Arial", 14);
            Javad.resizeTextForBestFit = true;
            Javad.color = Color.Lerp(Color.red, Color.yellow, 0.6f);
        }
        gameObject.transform.rotation = Quaternion.Euler(0, 0, angel + 180);
        ActiveRectTransform.anchoredPosition = Radius * Direction;
    }
    static void allmakeitright(GameObject G, int direction, bool ismagic)
    {
        AnchorRight(G, direction, ismagic);
        MakeItRight(G, direction, ismagic);
    }
    /*[MenuItem("fucker/Change #&b")]
    static void hapalan()
    {
        allmakeitright(Selection.activeGameObject);
        //Selection.activeGameObject.GetComponent<DirectionShowerHandler>().MakeItRight();
    }
    [MenuItem("fucker/justhapalan")]
    static void justhapalan()
    {
        MakeItRight(Selection.activeGameObject);
    }
    /*[MenuItem("fucker/allhapalan ")]
    static void allhapalan()
    {
        
        DirectionShowerHandler[] DSH = GameObject.FindObjectsOfType<DirectionShowerHandler>();
        /*for (int i = 0; i < DSH.Length; ++i)
            DSH[i].MakeItRight();
    }   */
    public UnityEngine.Events.UnityEvent mouseover_event_;
    public Image image_;
    public Direction direction_;
    public bool ifmagic_;
    public UnityEngine.UI.Selectable K;
    public bool selected;
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(direction_.direction.ToString());
        GetComponentInParent<MainScript>().SelectDirectionShower(this);
        
    }



    // Use this for initialization
    public void Set(int direction) {
        image_ = gameObject.GetComponent<Image>();
        if(direction > 7)
        {
            ifmagic_ = true;
            direction_ = new Direction(direction - 7);
        }
        else
        {
            ifmagic_ = false;
            direction_ = new Direction(direction);
        }
        allmakeitright(gameObject, direction_.direction, ifmagic_);//.direction ?
    }
	
	// Update is called once per frame
	void Update () {
        if (selected)
            K.Select();
	}
}
