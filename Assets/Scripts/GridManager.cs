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
    public CellObject template;

    public TetrominoManager blockmanager;

    // defined in Unity
    public int width;
    public int height;

    bool blockonscreen = false;
    bool gameover = false;
    
    // creates grid of Cells by creating "height" Rows of "width" size
    void SpawnGrid()
    {
        for (int y = 0; y < height; y++){
            RowObject row = new RowObject(y, width, template);
            grid.Add(row);
        }
    }

    // search through grid for cell at coords and return it
    CellObject getCellAtPos(Vector2 coords)
    {
        CellObject cellToReturn = null;

        foreach (RowObject row in grid){
            if (row.rownum == coords.y){
                foreach (CellObject cell in row.row){
                    if (cell.pos.x == coords.x){
                        cellToReturn = cell;
                        break;
                    }
                }
            }
        }
        return cellToReturn;
    }

   

    Tetromino SpawnBlock(){
        // pick random tetromino
        Tetromino block = blockmanager.getRandomBlock();

        // spawn location is 5, height
        CellObject spawn = getCellAtPos(new Vector2(5, 16));
        spawn.changeColor(block.BlockColor);

        // update origin in Tetromino
        block.origin = spawn.pos;
        
        // update positions in Tetromino, get appropiate cell, set color
        for (int i = 0; i < 3; i++)
        {
            block.points[i] += block.origin;
            CellObject cell = getCellAtPos(block.points[i]);
            cell.changeColor(block.BlockColor);
        }

        return block;
    }

    void MoveBlockDown(Tetromino block)
    {
        // reset original origin to white
        CellObject origin = getCellAtPos(block.origin);
        origin.changeColor(Color.black);

        // move origin down 1
        block.origin.y -= 1;
        origin = getCellAtPos(block.origin);
        origin.changeColor(block.BlockColor);

        for (int i = 0; i < 3; i++)
        {
            Vector2 point = block.points[i];
            CellObject cell = getCellAtPos(point);
            cell.changeColor(Color.white);
            
            point.y -= 1;
            block.points[i] = point;
            cell = getCellAtPos(point);
            cell.changeColor(block.BlockColor);
        }
    }





    IEnumerator TimeDelay(float time, Tetromino block)
    {
        yield return new WaitForSeconds(time);
        MoveBlockDown(block);
        yield return new WaitForSeconds(time);
        MoveBlockDown(block);
        yield return new WaitForSeconds(time);
        MoveBlockDown(block);
        yield return new WaitForSeconds(time);
        MoveBlockDown(block);
        yield return new WaitForSeconds(time);
        MoveBlockDown(block);
        yield return new WaitForSeconds(time);
        MoveBlockDown(block);
        Tetromino block2 = SpawnBlock();
        yield return new WaitForSeconds(time);
        MoveBlockDown(block2);
        yield return new WaitForSeconds(time);
        MoveBlockDown(block2);
        yield return new WaitForSeconds(time);
        MoveBlockDown(block2);
        yield return new WaitForSeconds(time);
        MoveBlockDown(block2);
        yield return new WaitForSeconds(time);
        MoveBlockDown(block2);
        yield return new WaitForSeconds(time);
        MoveBlockDown(block2);
    }






    // Use this for initialization
	void Start () {
        SpawnGrid();
        Tetromino block = SpawnBlock();
        StartCoroutine(TimeDelay(2, block));


        //while (!gameover)
        //{
        //  SpawnBlock();


        //}
    }

    void Update()
    {
        


    }
}


