using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Tetromino {
    public Color BlockColor;

    public Vector2 origin = new Vector2(0, 0);
    public List<Vector2> points = new List<Vector2>();

    // keep copy of local grid coordinates to do rotations
    public List<Vector2> localpoints = new List<Vector2>();

    public bool allowrotation = false;

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

    public Tetromino(Vector2 point, Vector2 point2, Vector2 point3, bool rotation)
    {
        if (rotation)
        {
            allowrotation = true;
        }

        points.Add(point);
        points.Add(point2);
        points.Add(point3);

        foreach (Vector2 coord in points)
        {
            localpoints.Add(coord);
        }
    }

   // grid position update happens in GridManager
    public List<Vector2> RotateCoords90(char Sign)
    {
        int Ysign, Xsign;

        List<Vector2> newpoints = new List<Vector2>();

        if (Sign == 'N')
        {
            Ysign = -1;
            Xsign = 1;
        }
        else
        {
            Ysign = 1;
            Xsign = -1;
        }

        // formula is either (-y, x) or (y, -x) 
        for (int i = 0; i < 3; i++)
        {
            Vector2 newpoint = localpoints[i];
            newpoint.x = localpoints[i].y * Xsign;
            newpoint.y = localpoints[i].x * Ysign;
            localpoints[i] = newpoint;
        }
        return localpoints;
    }
}