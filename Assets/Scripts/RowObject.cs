using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowObject : MonoBehaviour {

    public List<CellObject> row = new List<CellObject>();

    public int rownum; 
	
    public int ClearRow()
    {
        bool rowoccupied = true;

        foreach (CellObject cell in row)
        {
            if (cell.occupied == false)
            {
                rowoccupied = false;
                break;
            }
        }

        if (rowoccupied)
        {
            foreach (CellObject cell in row)
            {
                cell.changeColor(Color.white);
                cell.occupied = false;
            }
            return rownum;
        }

        return -1;
    }

    public RowObject(int y, int width, CellObject template) {
        rownum = y;

        // used to give objects in inspector a name
        GameObject holder = new GameObject();
        holder.name = "row " + y;

        // add cells to RowObject and Instantiate them in game
        for (int x = 0; x < width; x++){
            CellObject newCell = Instantiate(template, Vector3.zero, Quaternion.identity) as CellObject;

            // store grid pos in CellObject
            newCell.pos = new Vector2(x, y);

            newCell.transform.position = new Vector3(x, y, 0);

            // make holder parent of newCell to organize scene list 
            newCell.transform.parent = holder.transform;

            row.Add(newCell);
        }
    }
}
