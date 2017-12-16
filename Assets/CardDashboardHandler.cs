using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDashboardHandler : MonoBehaviour {

    private List<CardData> carddata_;
    private List<CardVisualizer> cardvisualizer_;
    public static CardDashboardHandler instance_;
    static public bool mouse_over_;/// <summary>
    /// //////////////////////////////////////
    /// </summary>
    public GameObject card_prefab_;
    public CardData[] cards_references_;//test
    public int radius;
    public int border_;
    private void Start()
    {
        carddata_ = new List<CardData>();
        cardvisualizer_ = new List<CardVisualizer>();
        Draw(new List<CardData>(cards_references_));
        instance_ = this;
    }
    private void PutCardVisulizersInPosition()
    {
        float Width = gameObject.GetComponent<RectTransform>().rect.width;
        float Height = gameObject.GetComponent<RectTransform>().rect.height;
        float need = carddata_.Count * (card_prefab_.GetComponent<RectTransform>().rect.width + border_);

        if (need > Width)
            for (int i = 0; i < carddata_.Count; ++i)
            {
                radius = carddata_.Count;
                RectTransform trans = (RectTransform)cardvisualizer_[i].transform;
                trans.anchorMax = new Vector2(0.5f, -radius);
                trans.anchorMin = new Vector2(0.5f, -radius);
                float maxangel = Mathf.Atan2(Width*8 / 20, radius * Height);
                float angel = maxangel - ((i+1) * maxangel * 2 / (1+carddata_.Count));
                Vector2 Direction = new Vector2(-Mathf.Sin(angel - Mathf.Atan2(trans.rect.width / 2, radius * Height)), Mathf.Cos(angel - Mathf.Atan2(trans.rect.width / 2, radius * Height)));
                trans.anchoredPosition = radius * Height * Direction;// + new Vector2(trans.rect.width,0); // 180 / Mathf.PI
                trans.rotation = Quaternion.EulerAngles(0, 0, angel);
            }
        else
            for (int i = 0; i < carddata_.Count; ++i)
            {
                RectTransform trans = (RectTransform)cardvisualizer_[i].transform;
                trans.anchoredPosition = new Vector2((-need / 2) - border_ + (i+1) * (trans.rect.width+border_),0);
            }
    }
    private void AddCard(CardData card)
    {
        CardVisualizer vis = Object.Instantiate(card_prefab_, gameObject.transform).GetComponent<CardVisualizer>();
        carddata_.Add(card);
        cardvisualizer_.Add(vis);
        vis.Visualize(card, true);
    }
    public void Draw(CardData card)
    {
        AddCard(card);
        PutCardVisulizersInPosition();
    }
    public void Draw(List<CardData> card)
    {
        for(int i = 0; i < card.Count; ++i)
            AddCard(card[i]);
        PutCardVisulizersInPosition();
    }
    public void Remove(CardData data,CardVisualizer cdvis)
    {
        carddata_.Remove(data);
        cardvisualizer_.Remove(cdvis);
        GameObject.Destroy(cdvis.gameObject);
        PutCardVisulizersInPosition();
    }
}
