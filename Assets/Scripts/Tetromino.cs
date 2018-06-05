using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tetromino {
    public Color BlockColor;

    public Vector2 origin = new Vector2(0, 0);
    public List<Vector2> points = new List<Vector2>();

    public List<Vector2> GetAllPoints()
    {
        List<Vector2> returnlist = new List<Vector2>();
        returnlist.Add(origin);

        foreach (Vector2 point in points)
        {
            returnlist.Add(point);
        }

        return returnlist;
    }



    public Tetromino(Vector2 point, Vector2 point2, Vector2 point3)
    {
        points.Add(point);
        points.Add(point2);
        points.Add(point3);
    }
}