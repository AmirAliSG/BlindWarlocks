using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformGameBoard : MonoBehaviour {
    private void OnMouseUpAsButton()
    {
        GameObject.Find("GameBoard").SendMessage("WallClicked");
    }
}
