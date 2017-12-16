using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HexHandler : MonoBehaviour {

    private void OnMouseEnter()
    {
        GetComponent<SpriteRenderer>().color = Color.gray;
    }
    private void OnMouseExit()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }
    private void OnMouseUpAsButton()
    {
        
    }
    // Use this for initialization
    void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
