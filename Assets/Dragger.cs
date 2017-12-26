using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dragger : MonoBehaviour {
    Vector3 screenPoint;
    Vector3 offset;
    // Use this for initialization
    void OnMouseDown()
    {
        screenPoint = Camera.main.WorldToScreenPoint(transform.parent.position);


        offset = transform.parent.position - Camera.main.ScreenToWorldPoint(
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

    }


    void OnMouseDrag()
    {
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);


        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.parent.position = curPosition;

    }
}
