using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CellObject is individual cell of grid
public class CellObject : MonoBehaviour {

    public Vector2 pos;

    public void changeColor(Color color)
    {
        // get renderer. renderer contains material, which displays color
        Renderer r = GetComponent<Renderer>();

        r.material.SetColor("_Color", color);
    }

}