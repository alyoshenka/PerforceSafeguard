using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// represents a game tile
public class Tile : MonoBehaviour
{
    public TileType type;
    public Index idx;

    [System.NonSerialized]
    public GameObject obj;

    public static Dictionary<TileType, Color> colors; // could be put in struct
    public static Dictionary<TileType, GameObject> objs;

    Material mat;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
    }

    public void Set(TileType _type, Index _idx, GameObject _obj)
    {
        type = _type;
        idx = _idx;
        obj = _obj;

        mat = obj.GetComponent<Renderer>().material;
    }

    private void OnMouseDown()
    {
        // change type
        Color c;
        type = MapGenerator.currentSelection;
        colors.TryGetValue(type, out c);
        mat.color = c;
    }

    private void OnMouseEnter()
    {
        if (Input.GetMouseButton(0)) { OnMouseDown(); } // drag select
        else
        {
            // make hover opaque (doesn't work)
            Color c = mat.color;
            c.a = 255;
            mat.color = c;
        }
    }

    private void OnMouseExit()
    {
        // un highlight
    }
}

