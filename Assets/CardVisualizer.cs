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
    [SerializeField]
    private GameObject invis_child_;
    public Text name_;
    private bool ismoving_;
    public bool ishandmode_;
    private CardData data_;
    private Vector3 firstplace_,childfirstplace_;
    private Quaternion firstrotate_,childfirstrotation_;
    public float scale_target_, upward_distance_, speed_;
    public void Visualize(CardData data,bool ishandmode)
    {
        data_ = data;
        background_.sprite = data.Img;
        name_.text = data.name;
        ishandmode_ = ishandmode;
        firstplace_ = transform.position;
        firstrotate_ = transform.rotation;
        childfirstplace_ = invis_child_.transform.position;
        childfirstrotation_ = invis_child_.transform.rotation;
        upward_distance_ = gameObject.GetComponent<RectTransform>().rect.height;
        ismoving_ = false;
    }
    private void SetBackInvisChild()
    {
        invis_child_.transform.position = childfirstplace_;
        invis_child_.transform.rotation = childfirstrotation_;
    }
    private void SetBackTransform()
    {
        transform.SetPositionAndRotation(firstplace_, firstrotate_);
        transform.localScale = Vector3.one;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {   
        if (ishandmode_)
        {
            firstplace_ = transform.position;
            firstrotate_ = transform.rotation;
            childfirstplace_ = invis_child_.transform.position;
            childfirstrotation_ = invis_child_.transform.rotation;
            transform.rotation = new Quaternion(0, 0, 0, 0);
            SetBackInvisChild();
            ismoving_ = true;
        }
        //else
    }
    public bool Move(Vector3 destination)
    {
        if (Vector3.Distance(transform.position, destination) < (float)speed_ * Time.deltaTime)
        {
            transform.position = destination;
            SetBackInvisChild();
            return true;
        }
        transform.Translate(speed_ * Time.deltaTime * (destination - transform.position));
        if (transform.localScale.x < scale_target_)
            transform.localScale = transform.localScale + Vector3.one * 0.1f;
        else
            transform.localScale = scale_target_*Vector3.one;
        SetBackInvisChild();
        return false;
    }
    private void Update()
    {
        if (ismoving_)
            if (Move(firstplace_ + (Vector3.up * upward_distance_)))
                ismoving_ = false;      
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        //Debug.Log(MainScript.HexPositionu(new Vector3(eventData.position.x, eventData.position.y)).ToString() + "  " + MainScript.HexPositionv(new Vector3(eventData.position.x, eventData.position.y)).ToString());
        if (ishandmode_)
        {
            ismoving_ = false;
            SetBackTransform();
            SetBackInvisChild();
            MainScript.instance_.SelectCardFromHand(data_, this);
        }
        else
            MainScript.instance_.UnselectCard(data_, this);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Time.
        //yield return new WaitForSeconds(1);
        if (ishandmode_)
        {
            ismoving_ = false;
            SetBackTransform();
            SetBackInvisChild();
        }
    }
}
