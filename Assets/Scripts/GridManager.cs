using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    

    // create grid composed of List<CellObjects>. this is to implement
    // "rows". each row will be its own list of CellObjects, and the grid will
    // have height amount of rows
    List<RowObject> grid = new List<RowObject>();

    // attached to prefab in Unity. Used as templates to spawn cells in-game
    // (needs to be Instantiated in order to appear in-game).
    // this is in GridManager because MonoBehavior only wants
    public CellObject template;

    public TetrominoManager blockmanager;

    // defined in Unity
    public int width;
    public int height;

    public bool blockonscreen = false;
    public bool gameover = false;
    

    // creates grid of Cells width by height
    void SpawnGrid()
    {
        for (int y = 0; y < height; y++){

            RowObject row = new RowObject(y, width, template);
            grid.Add(row);

        }
    }

    CellObject getCellAtPos(Vector2 coords)
    {
        CellObject cellToReturn = null;
        foreach (RowObject row in grid){
            if (row.rownum == coords.y){
                foreach (CellObject cell in row.row){
                    if (cell.pos.x == coords.x){
                        cellToReturn = cell;
                    }
                }

            }

        }




        return cellToReturn;
    }

    void SpawnBlock(){
        // pick random tetromino
        Tetronimo block = blockmanager.getRandomBlock();
        CellObject spawn = getCellAtPos(new Vector2(5, height));
        block.origin = spawn.pos;
        foreach (Vector2 points in block.points){
            points.x = block.origin.x


        }
    }








    // Use this for initialization
	void Start () {
        SpawnGrid();


       
        //blockOrigin.changeColor('R');

        
        //while (!gameover)
        //{
          //  SpawnBlock();


        //}
	}
    void Update()
    {
        //foreach


    }
}


