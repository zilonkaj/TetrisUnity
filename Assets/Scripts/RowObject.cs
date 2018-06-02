using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RowObject : MonoBehaviour {

    public List<CellObject> row = new List<CellObject>();
    public int rownum; 
	
    public RowObject(int rownum, int width, CellObject template) {
        // assign rownum
        this.rownum = rownum;
        GameObject holder = new GameObject();
        holder.name = "row " + rownum;

        for (int i = 0; i < width; i++){

            CellObject newCell = Instantiate(template, Vector3.zero, Quaternion.identity) as CellObject;

            // store pos in CellObject
            newCell.pos = new Vector2(i, rownum);

            // for now, using same coord system as in game. will be modified later
            newCell.transform.position = new Vector3(i, rownum, 0);
            newCell.transform.parent = holder.transform;

            // add to List
            row.Add(newCell);


        }
    }
}
