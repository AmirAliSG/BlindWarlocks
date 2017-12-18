using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DirectionShowerHandler : MonoBehaviour, IPointerClickHandler {
    const float Radius_Proportion = 8f;
    const int border_Proportion = 20;
    public static void AnchorRight(GameObject gameObject)
    {
        float Canvas_Height = gameObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.height;
        float Canvas_Width = gameObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.width;
        int ismagic = 1;
        if (gameObject.name[0] == 'M')
            ismagic += 2;
        float Radius = Canvas_Height / Radius_Proportion;
        float borderdistance = Canvas_Height / border_Proportion;
        RectTransform ActiveRectTransform = gameObject.GetComponent<RectTransform>();
        ActiveRectTransform.anchorMax = new Vector2(Radius * 3 / (2 * Canvas_Width), 1f - (float)(Radius + borderdistance) * ismagic / Canvas_Height);
        ActiveRectTransform.anchorMin = new Vector2(Radius * 3 / (2 * Canvas_Width), 1f - (float)(Radius + borderdistance) * ismagic / Canvas_Height);
    }
    public static void MakeItRight(GameObject gameObject)
    {

        float Canvas_Height = gameObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.height;
        float Canvas_Width = gameObject.GetComponentInParent<Canvas>().GetComponent<RectTransform>().rect.width;
        RectTransform ActiveRectTransform = gameObject.GetComponent<RectTransform>();
        int ismagic = 1;
        float borderdistance = Canvas_Height / border_Proportion;
        string a = gameObject.name;
        //GameObject.Find("Canvas").GetComponent<RectTransform>.
        float Radius = Canvas_Height / Radius_Proportion;
        if (a[0] == 'M')
            ismagic += 2;

        //Selection.activeGameObject.transform.position += Vector3.forward * 10;

        float angel = ((int)a[a.Length - 2] - (int)'0') * 60;


        Vector3 Direction = new Vector3(Mathf.Cos((angel - 90) * Mathf.PI / 180), Mathf.Sin((angel - 90) * Mathf.PI / 180), 0);
        if (a[a.Length - 2] == '7')
        {
            Direction = Vector3.zero;
            angel = 0;
        }
        else
            gameObject.transform.rotation = Quaternion.Euler(0, 0, angel + 180);
        ActiveRectTransform.anchoredPosition = Radius * Direction;
    }
    static void allmakeitright(GameObject G)
    {
        AnchorRight(G);
        MakeItRight(G);
    }
    [MenuItem("fucker/Change #&b")]
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
    public int direction_;
    public bool ifmagic_;
    public UnityEngine.UI.Selectable K;
    public bool selected;
    public void OnPointerClick(PointerEventData eventData)
    {
        GetComponentInParent<MainScript>().SelectDirectionShower(this);
        
    }



    // Use this for initialization
    void Start () {
        image_ = gameObject.GetComponent<Image>();
        direction_ = (int)gameObject.name[gameObject.name.Length - 2]-'0';
        ifmagic_ = false;
        if (gameObject.name[0] == 'M')
            ifmagic_ = true;
        allmakeitright(gameObject);
    }
	
	// Update is called once per frame
	void Update () {
        if (selected)
            K.Select();
	}
}
