using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tetromino {
    public Color BlockColor;

    bool isFalling = true;

    public Vector2 origin = new Vector2(0, 0);
    public List<Vector2> points = new List<Vector2>();

    public Tetromino(Vector2 point, Vector2 point2, Vector2 point3)
    {
        points.Add(point);
        points.Add(point2);
        points.Add(point3);
    }
}