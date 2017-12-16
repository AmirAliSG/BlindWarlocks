using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class GameBoardHandler : MonoBehaviour {
    public GameObject Hexagon_;
    private HexHandler[][] Tiles_;
    public GameObject Wall_;
    public float HexSize = 0.86f;
    public float border = 0.1f;
    static public int DirectionTranslate(int direction)
    {
        return ((2 * (int)(direction / 4)) - 1) * (1 - ((direction % 3) % 2));
    }
    public GameObject Get(int x, int y)
    {
        if (Tiles_[x][y] != null)
            return Tiles_[x][y].gameObject;
        else
            return Wall_;
    }
    public void WallClicked()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit[] hits = Physics.RaycastAll(ray);
        //Physics.
        Debug.Log(hits.Length.ToString());
        Debug.Log((Vector3.Distance(transform.position, hits[0].point) / (HexSize + border)).ToString());
        
    }
    public GameObject Next(int x, int y, int direction)
    {
        return Get(x + DirectionTranslate(((direction + 3) % 6) + 1), y + DirectionTranslate(direction));
        //1:x-z:x
        //2:x-y
        //3:z-y:-y
        //4:z-x:-x
        //5:y-x
        //6:y-z:y
        //x: + + 0 - - 0
        //y: 0 - - 0 + +
        //a: 1 2 3 4 5 6
        //System.IO.Path.
    }
    public void CreateTile()
    {

    }
    private void OnMouseUpAsButton()
    {
        Debug.Log("ghoorbaghe");
    }
    // Use this for initialization
    void Start () {
        /*Wall = Object.Instantiate(Hexagon, transform);
        Wall.transform.localScale = new Vector3(10, 10);
        Wall.GetComponent<SpriteRenderer>().color = Color.black;
        Object.Destroy(Wall.GetComponent<HexHandler>());*/
        

    }
	
	// Update is called once per frame
	void Update () {
        
    }
}
