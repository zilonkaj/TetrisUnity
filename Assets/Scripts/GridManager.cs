using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    
    public bool gameinProgress;

    // defined in Unity
    public int width;
    public int height;

    // create grid composed of List<CellObjects>. this is to implement
    // "rows". each row will be its own list of CellObjects, and the grid will
    // have height amount of rows
    List<RowObject> grid = new List<RowObject>();
    RowObject row21 = null;

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
        }

        return spawnblock;
    }

    // 0 is no collision. 1 is collision. 2 is side wall collision
    int CheckCollision(Tetromino activeblock, int subx, int suby)
    {
        List<Vector2> allpoints = activeblock.GetAllPoints();

        foreach (Vector2 point in allpoints)
        {
            Vector2 testpoint = new Vector2(point.x - (subx), point.y - (suby));
            CellObject testcell = getCellAtPos(testpoint);

            if (testcell == null)
            {
                if (testpoint.y >= 0)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }

            if (testcell.occupied)
            {
                return 1;
            }
        }

        return 0;
    }

    void MoveBlock(Tetromino activeblock, char Direction)
    {
        int subtractx, subtracty, collision, clearedrow;

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
        else if (Direction == 'r')
        {
            subtractx = -1;
            subtracty = 0;
        }
        else
        {
            subtractx = 0;
            subtracty = 0;
            int result = 0;
            int forresult;

            for (forresult = 0; result == 0; forresult++)
            {
                result = CheckCollision(activeblock, subtractx, forresult);
            }


            UpdateCells(activeblock, subtractx, forresult - 2);
            MoveBlock(activeblock, 'd');
        }

        collision = CheckCollision(activeblock, subtractx, subtracty);


        // if collision, spawn new block
        if (collision == 1)
        {
            foreach (Vector2 finalpoint in activeblock.GetAllPoints())
            {
                CellObject finalCell = getCellAtPos(finalpoint);
                finalCell.occupied = true;
            }

            foreach (RowObject row in grid)
            {
                clearedrow = row.ClearRow();
                if (clearedrow != -1)
                {
                    MoveRowsDown(clearedrow);
                }
            }

            block = SpawnBlock();
            return;
        }

        if (collision == 2)
        {
            return;
        }

        UpdateCells(activeblock, subtractx, subtracty);
    }

   

    void UpdateCells(Tetromino activeblock, int subx, int suby)
    {
        List<CellObject> prevCells = new List<CellObject>();
        List<CellObject> newCells = new List<CellObject>();

        // set all prev cells to white
        prevCells.Add(getCellAtPos(activeblock.origin));

        for (int i = 0; i < 3; i++)
        {
            prevCells.Add(getCellAtPos(activeblock.points[i]));
        }

        foreach (CellObject prevcell in prevCells)
        {
            prevcell.changeColor(Color.white);
        }

        // subtract/add change in direction
        activeblock.origin.x -= (subx);
        activeblock.origin.y -= (suby);
        newCells.Add(getCellAtPos(activeblock.origin));

        for (int i = 0; i < 3; i++)
        {
            Vector2 prevpoint = activeblock.points[i];
            prevpoint.x -= (subx);
            prevpoint.y -= (suby);
            activeblock.points[i] = prevpoint;
            newCells.Add(getCellAtPos(activeblock.points[i]));
        }

        // set new cells to BlockColor
        foreach (CellObject newcell in newCells)
        {
            newcell.changeColor(activeblock.BlockColor);
        }
    }

    void MoveRowsDown(int startrow)
    {
        RowObject currentrow = null;
        RowObject rowabove = null;

        for (int r = startrow; r < height - 1; r++)
        {

            // find two rows to swap
            foreach (RowObject row in grid)
            {
                if (row.rownum == r)
                {
                    currentrow = row;
                }
                if (row.rownum == (r + 1))
                {
                    rowabove = row;
                    break;
                }
            }

            foreach (CellObject currentcell in currentrow.row)
            {
                foreach (CellObject abovecell in rowabove.row)
                {
                    if (currentcell.pos.x == abovecell.pos.x)
                    {
                        currentcell.occupied = abovecell.occupied;
                        currentcell.changeColor(abovecell.GetColor());
                    }
                }
            }

            foreach (CellObject abovecell in rowabove.row)
            {
                abovecell.occupied = false;
                abovecell.changeColor(Color.white);
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

    // stop game if gameover
    void CheckForGameOver()
    {
        foreach (CellObject cell in row21.row)
        {
            if (cell.occupied && gameinProgress)
            {
                print("game over");
                gameinProgress = false;
            }
        }
    }

    // Use this for initialization
	void Start () {
        gameinProgress = true;
        SpawnGrid();

        // get row 21 (game over row)
        foreach (RowObject row in grid)
        {
            if (row.rownum == 21)
            {
                row21 = row;
            }
        }

        block = SpawnBlock();
        StartCoroutine(TimeDelay(1f)); 
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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            MoveBlock(block, 'S');
        }

        CheckForGameOver();
    }
}


