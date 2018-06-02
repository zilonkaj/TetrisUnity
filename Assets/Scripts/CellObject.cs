using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CellObject is individual cell of grid
public class CellObject : MonoBehaviour {

    public Vector2 pos;
    public bool isBlock = false;

    public void changeColor(char color)
    {
        // get renderer. renderer contains material, which displays color
        Renderer r = GetComponent<Renderer>();

        if (color == 'R')
            r.material.SetColor("_Color", Color.red);
        else if (color == 'W')
            r.material.SetColor("_Color", Color.white);
    }

}