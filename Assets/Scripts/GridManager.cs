using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    
    public bool gameinProgress;
    bool gameover = false;

    // defined in Unity
    public int width;
    public int height;

    // create grid composed of List<CellObjects>. this is to implement
    // "rows". each row will be its own list of CellObjects, and the grid will
    // have height amount of rows
    List<RowObject> grid = new List<RowObject>();

    // attached to prefab in Unity. Used as templates to spawn cells in-game
    // (needs to be Instantiated in order to appear in-game).
    public CellObject template;

    public TetrominoManager blockmanager;

    // current falling block
    public Tetromino block;


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
        
        // if block is offscreen and above 0
        if (cellToReturn == null && coords.y >= 0)
        {
            cellToReturn = getCellAtPos(new Vector2(coords.x, coords.y - 1));
        }

        return cellToReturn;
    }

    Tetromino SpawnBlock(){
        // pick random tetromino
        Tetromino spawnblock = blockmanager.getRandomBlock();

        // spawn location is 5, height - 3
        CellObject spawn = getCellAtPos(new Vector2(5, height - 3));
        spawn.changeColor(spawnblock.BlockColor);

        // update origin in Tetromino
        spawnblock.origin = spawn.pos;
        
        // update positions in Tetromino, get appropiate cell, set color
        for (int i = 0; i < 3; i++)
        {
            spawnblock.points[i] += spawnblock.origin;
            CellObject cell = getCellAtPos(spawnblock.points[i]);

            // if cell offscreen, skip color change
            if (cell.pos != spawnblock.points[i])
            {
                continue;
            }
            else
            {
                cell.changeColor(spawnblock.BlockColor);
            }

            cell.occupied = true;
        }

        return spawnblock;
    }

    void MoveBlock(Tetromino ourblock, char Direction)
    {
        int subtractx, subtracty;

        if (Direction == 'd')
        {
            subtractx = 0;
            subtracty = 1;
        }
        else if (Direction == 'l')
        {
            subtractx = 1;
            subtracty = 0;
        }
        else
        {
            subtractx = -1;
            subtracty = 0;
        }


        // check space below to see if occupied. if occupied, block collided with another
        Vector2 testpoint = new Vector2(ourblock.origin.x - (subtractx), ourblock.origin.y - (subtracty));
        CellObject testcell = getCellAtPos(testpoint);
        CellObject origin = getCellAtPos(ourblock.origin);
       
        // if cell below not bottom or not occupied 
        if (testcell != null && !testcell.occupied)
        {
            // reset previous origin to white
            origin.changeColor(Color.white);

            // move origin
            ourblock.origin.x -= (subtractx);
            ourblock.origin.y -= (subtracty);
            origin = getCellAtPos(ourblock.origin);
            origin.changeColor(ourblock.BlockColor);
        }

        // block collided, so spawn new one
        else
        {
            origin.occupied = true;

            block = SpawnBlock();
            return;
        }

        for (int i = 0; i < 3; i++)
        {
            testpoint = new Vector2(ourblock.points[i].x - (subtractx), ourblock.points[i].y - (subtracty));
            testcell = getCellAtPos(testpoint);

            if (testcell != null && !testcell.occupied)
            {
                Vector2 point = ourblock.points[i];
                CellObject cell = getCellAtPos(point);
                cell.changeColor(Color.white);

                point.x -= (subtractx);
                point.y -= (subtracty);
                ourblock.points[i] = point;
                cell = getCellAtPos(point);
                cell.changeColor(ourblock.BlockColor);

            }
            else
            {
                Vector2 point = new Vector2(ourblock.points[i].x, ourblock.points[i].y);
                CellObject cell = getCellAtPos(point);
                cell.occupied = true;
                block = SpawnBlock();
                return;
            }
        }
    }

    IEnumerator TimeDelay(float time)
    {
        while (gameinProgress){
            yield return new WaitForSeconds(time);
            MoveBlock(block, 'd');
        }
    }

    // Use this for initialization
	void Start () {
        gameinProgress = true;
        SpawnGrid();
        block = SpawnBlock();
        StartCoroutine(TimeDelay(.2f)); 
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow)){
            MoveBlock(block, 'l');
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveBlock(block, 'r');
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveBlock(block, 'd');
        }
    }
}


