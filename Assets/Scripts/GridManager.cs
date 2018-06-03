using System;
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
        
        // for when block spawns offscreen
        if (cellToReturn == null)
        {
            cellToReturn = getCellAtPos(new Vector2(coords.x, coords.y - 1));
        }

        return cellToReturn;
    }

   

    Tetromino SpawnBlock(){
        // pick random tetromino
        Tetromino block = blockmanager.getRandomBlock();

        // spawn location is 5, height - 1
        CellObject spawn = getCellAtPos(new Vector2(5, height));
        spawn.changeColor(block.BlockColor);
        spawn.occupied = true;

        // update origin in Tetromino
        block.origin = spawn.pos;
        
        // update positions in Tetromino, get appropiate cell, set color
        for (int i = 0; i < 3; i++)
        {
            block.points[i] += block.origin;
            CellObject cell = getCellAtPos(block.points[i]);

            // if cell offscreen, skip color change
            if (cell.pos != block.points[i])
            {
                continue;
            }
            else
            {
                cell.changeColor(block.BlockColor);
            }

            cell.occupied = true;

        }

        return block;
    }

    void MoveBlockDown(Tetromino block)
    {
        // check space below to see if occupied. if occupied, block collided with another
        Vector2 testpoint = new Vector2(block.origin.x, block.origin.y - 1);
        CellObject testcell = getCellAtPos(testpoint);

        // if cell below not occupied
        if (!testcell.occupied)
        {
            // reset original origin to white
            CellObject origin = getCellAtPos(block.origin);
            origin.changeColor(Color.white);
            origin.occupied = false;

            // move origin down 1
            block.origin.y -= 1;
            origin = getCellAtPos(block.origin);
            origin.changeColor(block.BlockColor);
            origin.occupied = true;
        }
        else
        {
            return;
        }

        for (int i = 0; i < 3; i++)
        {
            testpoint = new Vector2(block.points[i].x, block.points[i].y - 1);
            testcell = getCellAtPos(testpoint);

            if (!testcell.occupied)
            {
                Vector2 point = block.points[i];
                CellObject cell = getCellAtPos(point);
                cell.changeColor(Color.white);
                cell.occupied = false;

                point.y -= 1;
                block.points[i] = point;
                cell = getCellAtPos(point);

                // if cell offscreen, skip color change
                if (cell.pos != point)
                {
                    continue;
                }
                else
                {
                    cell.changeColor(block.BlockColor);
                }

                cell.occupied = true;
            }
            else
            {
                return;
            }
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
        try
        {
            Tetromino block2 = SpawnBlock();
        }
        catch (Exception e)
        {
            Debug.LogException(e, this);
        }

        //yield return new WaitForSeconds(time);
        //MoveBlockDown(block2);
        //yield return new WaitForSeconds(time);
        //MoveBlockDown(block2);
        //yield return new WaitForSeconds(time);
        //MoveBlockDown(block2);
        //yield return new WaitForSeconds(time);
        //MoveBlockDown(block2);
        //yield return new WaitForSeconds(time);
        //MoveBlockDown(block2);
        //yield return new WaitForSeconds(time);
        //MoveBlockDown(block2);
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


