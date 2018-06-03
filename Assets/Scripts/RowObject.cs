using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowObject : MonoBehaviour {

    public List<CellObject> row = new List<CellObject>();
    
    // which row this object represents
    public int rownum; 
	
    public RowObject(int y, int width, CellObject template) {
        // assign rownum
        rownum = y;

        // used to give objects in inspector a name
        GameObject holder = new GameObject();
        holder.name = "row " + y;

        // add cells to RowObject and Instantiate them in game
        for (int x = 0; x < width; x++){
            CellObject newCell = Instantiate(template, Vector3.zero, Quaternion.identity) as CellObject;

            // store grid pos in CellObject
            newCell.pos = new Vector2(x, y);

            // for now, using same coord system as in game. will be modified later
            newCell.transform.position = new Vector3(x, y, 0);

            // make holder parent of newCell to organize scene list 
            newCell.transform.parent = holder.transform;

            // add to List
            row.Add(newCell);
        }
    }
}
