using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridManager : MonoBehaviour {
    
    public bool gameinProgress = false;
    public bool gameover = false;

    public int score = 0;

    // defined in Unity
    public int width;
    public int height;

    // create grid composed of List<RowObjects>. 
    // each row will be a list of CellObjects, and the grid will
    // have height amount of rows
    List<RowObject> grid = new List<RowObject>();

    // row 21 is row above camera
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

    CellObject getCellAtPos(Vector2 coords)
    {
        CellObject cellToReturn = null;

        foreach (RowObject row in grid){
            if (row.rownum == coords.y)
            {
                foreach (CellObject cell in row.row)
                {
                    if (cell.pos.x == coords.x)
                    {
                        cellToReturn = cell;
                        break;
                    }
                }
                break;
            }
        }
        return cellToReturn;
    }

    Tetromino SpawnBlock(){
        
        Tetromino activeblock = blockmanager.getRandomBlock();

        // spawn location is 5, height - 3
        CellObject spawncell = getCellAtPos(new Vector2(5, height - 3));
        spawncell.changeColor(activeblock.BlockColor);

        // update origin in Tetromino
        activeblock.origin = spawncell.pos;
        
        // update positions, get appropiate cell, set color
        for (int i = 0; i < 3; i++)
        {
            activeblock.points[i] += activeblock.origin;
            CellObject cell = getCellAtPos(activeblock.points[i]);

            if (cell != null)
            {
                cell.changeColor(activeblock.BlockColor);
            }
        }

        return activeblock;
    }

    // 0 is no collision. 1 is collision. 2 is left wall collision, 3 is right wall collision
    int CheckCollision(List<Vector2> newpoints)
    {
        foreach (Vector2 point in newpoints)
        {
            CellObject testcell = getCellAtPos(point);

            if (testcell == null)
            {
                if (point.y >= 0)
                {
                    if (point.x < 0)
                    {
                        return 2;
                    }
                    else
                    {
                        return 3;   
                    }

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
        int subtractx, subtracty, collision;

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

        // create list to send to CheckCollision()
        List<Vector2> testpoints = new List<Vector2>();

        // add points
        foreach (Vector2 point in activeblock.points)
        {
            Vector2 newpoint = point;
            newpoint.x -= subtractx;
            newpoint.y -= subtracty;
            testpoints.Add(newpoint);
        }

        // make copy of testpoints in case no collision
        List<Vector2> newpoints = new List<Vector2>(testpoints);

        // add origin
        Vector2 neworigin = activeblock.origin;
        neworigin.x -= subtractx;
        neworigin.y -= subtracty;
        testpoints.Add(neworigin);

        collision = CheckCollision(testpoints);

        // if sidewall collision, ignore it
        if (collision > 1)
        {
            return;
        }
        if (collision == 1)
        {
            List<Vector2> prevpoints = new List<Vector2>(activeblock.points);
            prevpoints.Add(activeblock.origin);
            SetCellsOccupied(prevpoints);
            block = SpawnBlock();
            return;
        }
        else
        {
            UpdateCells(activeblock, testpoints);

            // change coords
            activeblock.origin = neworigin;
            activeblock.points = newpoints;
        }
    }

    void SpacePress(Tetromino activeblock)
    {
        int collisResult = -1;
        int distance;

        List<Vector2> newpoints = new List<Vector2>();
        List<Vector2> finalpoints = new List<Vector2>();

        for (distance = 0; collisResult != 1; distance++)
        {
            List<Vector2> testpoints = new List<Vector2>();

            foreach (Vector2 point in activeblock.GetAllPoints())
            {
                Vector2 testpoint = new Vector2(point.x, point.y - distance);
                testpoints.Add(testpoint);
            }

            collisResult = CheckCollision(testpoints);
            newpoints = testpoints;
        }

        // add 1 back because of grid start at 0 
        foreach (Vector2 point in newpoints)
        {
            Vector2 finalpoint = new Vector2(point.x, point.y + 1);
            finalpoints.Add(finalpoint);
        }

        SetCellsOccupied(finalpoints);
        UpdateCells(activeblock, finalpoints);
        block = SpawnBlock();
    }

    void SetCellsOccupied(List<Vector2> finalpoints)
    {
        foreach (Vector2 point in finalpoints)
        {
            CellObject finalCell = getCellAtPos(point);
            if (finalCell != null)
            {
                finalCell.occupied = true;   
            }
        }
    }

    // change what cells a Tetromino appears in from previous coords to newpoints
    void UpdateCells(Tetromino activeblock, List<Vector2> newpoints)
    {
        List<CellObject> newCells = new List<CellObject>();

        foreach (Vector2 point in activeblock.GetAllPoints())
        {
            CellObject cell = getCellAtPos(point);
            if (cell != null)
            {
                cell.changeColor(Color.white);
            }
        }

        foreach (Vector2 point in newpoints)
        {
            newCells.Add(getCellAtPos(point));
        }

        foreach (CellObject newcell in newCells)
        {
            if (newcell != null)
            {
                newcell.changeColor(activeblock.BlockColor);
            }
        }
    }

    void Rotate90(Tetromino activeblock, char sign)
    {
        if (!activeblock.allowrotation)
        {
            return;
        }

        List<Vector2> localpoints = activeblock.RotateCoords90(sign);
        List<Vector2> newpoints = new List<Vector2>();
        int collision;

        foreach (Vector2 point in localpoints)
        {
            Vector2 newpoint = point + activeblock.origin;
            newpoints.Add(newpoint);
        }

        // origin needs to be sent to CheckCollision & UpdateCells
        List<Vector2> updatepoints = new List<Vector2>(newpoints);
        updatepoints.Add(activeblock.origin);

        collision = CheckCollision(updatepoints);
        if (collision != 0)
        {
            if (collision == 2)
            {
                MoveBlock(activeblock, 'r');
            }
            if (collision == 3)
            {
                MoveBlock(activeblock, 'l');
            }

            Rotate90(activeblock, sign);
            return;
        }

        UpdateCells(activeblock, updatepoints);

        // replace non-origin points
        activeblock.points = newpoints;
    }

    // cascades if multiple rows full
    void MoveRowsDown(int startrow)
    {
        RowObject currentrow = null;
        RowObject rowabove = null;

        // height - 4 to ignore rows above camera
        for (int r = startrow; r < height - 4; r++)
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
                }

                // end loop early if both found
                if (currentrow != null && rowabove != null)
                {
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
                        break;
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
                gameinProgress = false;
                gameover = true;
                break;
            }
        }
    }

    void ClearBoard()
    {
        foreach (RowObject row in grid)
        {
            foreach (CellObject cell in row.row)
            {
                cell.changeColor(Color.white);
                cell.occupied = false;
            }
        }

    }

    void CheckForFullRow()
    {
        int fullrow;

        foreach (RowObject row in grid)
        {
            // ClearRow() returns num > 0 if its full
            fullrow = row.ClearRow();
            if (fullrow != -1)
            {
                score += 100;
                MoveRowsDown(fullrow);
                break;
            }
        }
    }

    public void StartGame()
    {
        // check if game already in progress
        if (!gameinProgress)
        {
            ClearBoard();
            gameinProgress = true;

            block = SpawnBlock();
            StartCoroutine(TimeDelay(1f));
        }
    }


    // Use this for initialization
	void Start () {

        gameinProgress = false;

        SpawnGrid();

        // get row 21 (game over row)
        foreach (RowObject row in grid)
        {
            if (row.rownum == 21)
            {
                row21 = row;
                break;
            }
        }
    }

    void Update()
    {
        if (gameinProgress)
        {
            CheckForGameOver();

            CheckForFullRow();

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
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
                SpacePress(block);
            }
            if (Input.GetKeyDown(KeyCode.Z))
            {
                Rotate90(block, 'P');
            }
            if (Input.GetKeyDown(KeyCode.X))
            {
                Rotate90(block, 'N');
            } 
        }

    }
}