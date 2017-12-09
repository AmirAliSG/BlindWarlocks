using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
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
    public string name;
    public List<CardData> hand;
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
    public UnityEngine.Events.UnityEvent Abdulah;
    [UnityEngine.Serialization.FormerlySerializedAsAttribute("cards")]
    public CardData[] cards_references_;
    public GameObject card_prefab_;

    [MenuItem("fucker/ Change")]
    static void hapalan()
    {
        int Canvas_Height = 502;
        int ismagic = 1;
        int borderdistance = Canvas_Height/30;
        //Abd.height
        string a = Selection.activeGameObject.name;
        //GameObject.Find("Canvas").GetComponent<RectTransform>.
        int Radius = Canvas_Height/6;
        if (a[0] == 'M')
            ismagic += 2;
        RectTransform ActiveRectTransform = Selection.activeGameObject.GetComponent<RectTransform>();
        ActiveRectTransform.anchorMax = new Vector2(0.12f,1f - (float)(Radius + borderdistance )* ismagic/Canvas_Height);
        ActiveRectTransform.anchorMin = new Vector2(0.12f,1f - (float)(Radius + borderdistance) * ismagic/Canvas_Height);
        //Selection.activeGameObject.transform.position += Vector3.forward * 10;
        
        int angel = ((int)a[a.Length - 2] - (int)'0') * 60;
        

        //Selection.activeGameObject.transform.Rotate(0, 0, angel);
        Selection.activeGameObject.transform.rotation = Quaternion.Euler(0, 0, angel);
        Vector3 Direction = new Vector3(Mathf.Cos((angel - 90) * Mathf.PI / 180), Mathf.Sin((angel - 90) * Mathf.PI / 180), 0);
        Selection.activeGameObject.GetComponent < RectTransform >().anchoredPosition = Radius * Direction;
        
        Debug.Log(Mathf.Sin((Mathf.PI*(angel-90))/180).ToString());
        Debug.Log((angel).ToString());
        
    }
    
    private Canvas canvas;
    private List<Pair<CardData,CardVisualizer>> hand_;
    public void MoveObject(Transform objecttransform,float speed,Vector3 destination)
    {
        
        //float distance = speed* Time.deltaTime 
          
        //if(Vector3.Distance(objecttransform.position,destination)
    }
    // Use this for initialization
    void Start() {
        hand_ = new List<Pair<CardData, CardVisualizer>>();
        canvas = FindObjectOfType<Canvas>();
        ShowInitialHand(new List<CardData>(cards_references_));
        
        Debug.Log(transform.position.ToString());
        //Selection.activeTransform.position;
    }
    private void Update()
    {
        //Debug.Log(Input.GetAxis("Horizontal").ToString());
    }
    private void ShowInitialHand(List<CardData> cards)
    {
        for (int i = 0; i < cards.Count; ++i)
            Draw(cards[i]);
        RefreshHandArrangements();
    }

    private void Draw(CardData card)
    {
        CardVisualizer vis = Instantiate(card_prefab_, canvas.transform).GetComponent<CardVisualizer>();
        hand_.Add(new Pair<CardData, CardVisualizer>(card, vis));
        vis.Visualize(card,true);
    }

    private void RefreshHandArrangements()
    {
        for(int i=0; i<hand_.Count; ++i)
        {
            RectTransform trans = (RectTransform)hand_[i].pItem2.transform;
            trans.anchoredPosition = new Vector2(i * (-trans.rect.width - 5), 0);
        }
    }
 }
