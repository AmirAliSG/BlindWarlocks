using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PairHex
{
    public int x;
    public int y;
    static public int DirectionTranslate(int direction)
    {
        return ((2 * (int)(direction / 4)) - 1) * (1 - ((direction % 3) % 2));
    }
    public int NextX(int direction)
    {
        return x + DirectionTranslate(((direction + 3) % 6) + 1);
    }
    public int NextY(int direction)
    {
        return y + DirectionTranslate(direction);
    }
    public PairHex Next(int direction)
    {
        return new PairHex(NextX(direction), NextY(direction));
        //1:x-z:x
        //2:x-y
        //3:z-y:-y
        //4:z-x:-x
        //5:y-x
        //6:y-z:y
        //x: + + 0 - - 0
        //y: 0 - - 0 + +
        //a: 1 2 3 4 5 6
    }
    public Vector3 PositionOf()
    {
        Vector3 XDirection = new Vector3(Mathf.Cos(330 * Mathf.PI / 180), Mathf.Sin(330 * Mathf.PI / 180));
        Vector3 YDirection = Vector3.down;
        //Debug.Log(((XDirection * x * (hexsize + border)) + (YDirection * y * (hexsize + border))).ToString());
        float Dis = GameBoardHandler.HexSize_ + GameBoardHandler.border_;
        return (XDirection * x * Dis) + (YDirection * y * Dis);
    }
    public PairHex(int X, int Y)
    {
        x = X;
        y = Y;
    }
    static public int Int(string str)
    {
        int s = 0;
        int i = 0;
        if (str[0] == '-')
            i++;
        for (; i < str.Length; i++)
        {
            s *= 10;
            s += ((int)str[i] - '0');
        }
        if(str[0] == '-')
            return s*-1;
        return s;
    }
    public PairHex(string A)
    {
        char[] B = new char[3];
        B[0] = ',';
        B[1] = '(';
        B[2] = ')';
        x = Int(A.Split(B)[1]);
        y = Int(A.Split(B)[2]);
    }
    public override string ToString()
    {
        return "(" + x.ToString() + "," + y.ToString() + ")";
    }
    public static bool operator ==(PairHex P, PairHex H)
    {
        if (P.x == H.x && P.y == H.y)
            return true;
        return false;
    }
    public static bool operator !=(PairHex P, PairHex H)
    {
        if (P.x == H.x && P.y == H.y)
            return false;
        return true;
    }
    
    public override bool Equals(object obj)
    {
        //       
        // See the full list of guidelines at
        //   http://go.microsoft.com/fwlink/?LinkID=85237  
        // and also the guidance for operator== at
        //   http://go.microsoft.com/fwlink/?LinkId=85238
        //

        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        if (obj.ToString() == this.ToString())
            return true;
        return false;
    }

    // override object.GetHashCode

    public override int GetHashCode()
    {
        return x * 100000 + y;
    }
}
public class GameBoardHandler : MonoBehaviour {
    public GameObject Hexagon_;
    public GameObject InputField;
    private bool IsSaving_;
    private bool IsLoading_;
    public int MAXPLAYERS = 6;
    //public 
    private Dictionary<PairHex, HexHandler> Tiles_;
    public GameObject Wall_;
    public static float HexSize_ = 0.86f;
    public static float border_ = 0.07f;
    public bool CreateMapMode_;
    public int player_number_;
    public string mapstring_;
   // public PairHex[][] players_area_;
    public HexHandler clicked_hex_;
    
    public void SetActiveHandler(bool issave)
    {
        if (issave)
        {
            if (IsLoading_)
            {
                IsLoading_ = false;
                InputField.SetActive(false);
            }
            else
            {
                IsSaving_ = true;
                InputField.SetActive(true);
                InputField.GetComponent<UnityEngine.UI.InputField>().placeholder.GetComponent<UnityEngine.UI.Text>().text = "Map name to save ...";
            }
        }
        else
        {
            if (IsSaving_)
            {
                IsSaving_ = false;
                InputField.SetActive(false);
            }
            else
            {
                IsLoading_ = true;
                InputField.SetActive(true);
                InputField.GetComponent<UnityEngine.UI.InputField>().placeholder.GetComponent<UnityEngine.UI.Text>().text = "Map name to load ...";
            }
        }
    }

    public void writetofile(string txt)
    {
        string path = "Assets/maps.txt";
        System.IO.TextWriter W = new System.IO.StreamWriter(path, true);
        W.Write(txt);
        W.Close();
    }
    public void Save(string name)
    {
        writetofile("\n"+name+".");
        foreach (PairHex I in Tiles_.Keys)
        {
            writetofile(Tiles_[I].player_.ToString() + I.ToString() + ".");
        }
        writetofile(mapstring_);
    }
    public void SaveToFile()
    {
        if (IsSaving_)
        {
            if (InputField.GetComponent<UnityEngine.UI.InputField>().text != "")
            {
                Save(InputField.GetComponent<UnityEngine.UI.InputField>().text);
                SetActiveHandler(false);
            }
        }
        else
            SetActiveHandler(true);
    }

    public void ReplaceLoaded(string[] Goospand)
    {
        List<PairHex> Keys;
        Keys = new List<PairHex>();
        foreach (PairHex P in Tiles_.Keys)
        {
            Keys.Add(P);
        }
        for(int i = 0; i < Keys.Count; i++)
        {
            DestroyTile(Keys[i]);
        }
        for (int i = 1; i < Goospand.Length - 1; ++i)
        {
            player_number_ = PairHex.Int(Goospand[i][0].ToString());
            CreateTile(new PairHex(Goospand[i].Substring(Goospand[i].IndexOf('('))));
        }
        mapstring_ = Goospand[Goospand.Length-1];
    }
    public bool Load(string name)
    {
        char[] B = new char[1];
        B[0] = '.';
        string path = "Assets/maps.txt";
        System.IO.StreamReader R = new System.IO.StreamReader(path);
        string[] Goospand;

        do
        {
            string A = R.ReadLine();
            if (A == null)
            {
                R.Close();
                return false;
            }
            Goospand = A.Split(B);
        } while (Goospand[0] != name);
        ReplaceLoaded(Goospand);
        R.Close();
        if (CreateMapMode_)
            GameObject.Find("PlayerNumberForHex").GetComponent<UnityEngine.UI.InputField>().text = "0";
        return true; 
    }
    public void LoadFromFile()
    {
        if (IsLoading_)
        {
            if (InputField.GetComponent<UnityEngine.UI.InputField>().text != "")
            {
                if (Load(InputField.GetComponent<UnityEngine.UI.InputField>().text))
                    SetActiveHandler(true);
                else
                    Debug.Log("NotFound");
            }
        }
        else
            SetActiveHandler(false);
    }
    
    public PairHex[] PlayerArea(int player)
    {
        char[] B = new char[1];
        B[0] = ':';
        char[] C = new char[1];
        C[0] = '&';
        string[] A = mapstring_.Substring(mapstring_.LastIndexOf(player.ToString() + ":")).Split(B)[1].Split(C);
        PairHex[] P = new PairHex[A.Length];
        for(int i = 0; i < A.Length; i++)
        {
            P[i] = new PairHex(A[i]);
        }
        return P;
    }
    public void AddToString(PairHex P,int player)
    {

        int PLACE = mapstring_.LastIndexOf(player.ToString() + ":") + 2;
        int LENGTH = mapstring_.Length - mapstring_.Substring(PLACE).Length;
        mapstring_ = mapstring_.Substring(0, LENGTH) + P.ToString() + "&" + mapstring_.Substring(PLACE);

    }
    public void ButtonClicked(int direction)
    {
        if (clicked_hex_ != null)
        {
            PairHex P = new PairHex(clicked_hex_.x_, clicked_hex_.y_).Next(direction);
            if (Tiles_.ContainsKey(P))
                DestroyTile(P);
            else
            {
                if(player_number_ != 0)
                {
                    AddToString(P, player_number_);
                }
                HexClicked(CreateTile(P));
            }
        }
    }
    public void HexUnclicked(HexHandler G)
    {
        G.Unclick();
        clicked_hex_ = null;
    }
    public void HexClicked(HexHandler G)
    {
        if(clicked_hex_ != null)
            clicked_hex_.Unclick();
        G.Click();
        clicked_hex_ = G;
    }
    
    public bool Available(PairHex P,int playernumber)
    {
        if(Tiles_.ContainsKey(P) && (Tiles_[P].player_ == 0 || playernumber == Tiles_[P].player_))
            return true;
        return false;
    }
    public GameObject Get(int x, int y)
    {
        if (Tiles_.ContainsKey(new PairHex(x,y)))
            return Tiles_[new PairHex(x,y)].gameObject;
        return Wall_;
    }
    //private ?

    public string MapName;///
    public void DestroyTile(PairHex P)
    {
        GameObject.Destroy(Tiles_[P].gameObject);
        Tiles_.Remove(P);
    }
    public HexHandler CreateTile(int x,int y)
    {
        return CreateTile(new PairHex(x,y));
    }
    public HexHandler CreateTile(PairHex P)
    {
        HexHandler H = GameObject.Instantiate(Hexagon_, transform).GetComponent<HexHandler>();
        H.Set(P.x, P.y,player_number_, P.PositionOf());
        Tiles_.Add(P, H);
        return H;
    }
    // Use this for initialization
    void Start () {
        HexHandler.LightBlue = Color.Lerp(Color.blue, Color.white, 0.5f);
        /*Wall = Object.Instantiate(Hexagon, transform);
        Wall.transform.localScale = new Vector3(10, 10);
        Wall.GetComponent<SpriteRenderer>().color = Color.black;
        Object.Destroy(Wall.GetComponent<HexHandler>());*/
        mapstring_ = "";
        for(int i = 1; i < MAXPLAYERS + 1; i++)
        {
            mapstring_ = mapstring_  + i.ToString() + "::";
        }
        HexSize_ = HexSize_ * transform.localScale.x;
        border_ = border_ * transform.localScale.x;
        Tiles_ = new Dictionary<PairHex, HexHandler>();
        clicked_hex_ = CreateTile(0, 0);
        if (CreateMapMode_)
        {
            IsSaving_ = false;
            IsLoading_ = false;
            UnityEngine.UI.InputField U= GameObject.Find("PlayerNumberForHex").GetComponent<UnityEngine.UI.InputField>();
            U.onValueChanged.AddListener(HexNumberChanged);
            U.text = "0";
        }
        else
        {
            Load(MapName);
            transform.position = new Vector3(-3.5f, 0, 0);
        }
        //HexClicked(CreateTile(0, 0));
        /* GameObject A = GameObject.Instantiate(Hexagon_, transform);
         A.transform.localScale = new Vector3(10, 10, 1);
         A.GetComponent<SpriteRenderer>().color = Color.black;
         A.transform.localPosition = new Vector3(0, 0, -0.1f);*/
    }
    public void HexNumberChanged (string value)
    {
        if (value == "")
            GameObject.Find("PlayerNumberForHex").GetComponent<UnityEngine.UI.InputField>().text = "0";
        else
        {
            char C = value[value.Length - 1];
            if (C < '0' || C > '9')
            {
                GameObject.Find("PlayerNumberForHex").GetComponent<UnityEngine.UI.InputField>().text = "0";
            }
            else
                player_number_ = PairHex.Int(value);
        }
    }
}
