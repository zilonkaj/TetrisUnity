using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoManager : MonoBehaviour {
    
    List<Tetromino> shapes = new List<Tetromino>();

    // list of possible Tetrominos. done manually because the inspector creates problems
    Tetromino I_Block = new Tetromino(new Vector2(0, 1), new Vector2(0, 2), new Vector2(0, 3));
    Tetromino L_Block = new Tetromino(new Vector2(1, 0), new Vector2(0, 1), new Vector2(0, 2));
    Tetromino J_Block = new Tetromino(new Vector2(-1, 0), new Vector2(0, 1), new Vector2(0, 2));
    Tetromino Plus = new Tetromino(new Vector2(0, 1), new Vector2(-1, 0), new Vector2(1, 0));
    Tetromino Z_Block = new Tetromino(new Vector2(-1, 1), new Vector2(0, 1), new Vector2(1, 0));
    Tetromino S_Block = new Tetromino(new Vector2(-1, 0), new Vector2(0, 1), new Vector2(1, 1));
    Tetromino Square = new Tetromino(new Vector2(0, 1), new Vector2(1, 0), new Vector2(1, 1));
    Tetromino T_Block = new Tetromino(new Vector2(0, 1), new Vector2(1, 1), new Vector2(-1, 1));

    // different color values assigned in inspector
    public List<Color> colors;

    public Tetromino getRandomBlock(){
        
        // Random.Range does not include last value (so from 0 - shapes.Count - 1)
        Tetromino block = shapes[Random.Range(0, shapes.Count)];
        
        // give block random color
        block.BlockColor = colors[Random.Range(0, colors.Count)];

        return block;
    }  

    void Start()
    {
        shapes.Add(I_Block);
        shapes.Add(L_Block);
        shapes.Add(J_Block);
        shapes.Add(Plus);
        //shapes.Add(Z_Block);
        //shapes.Add(S_Block);
        //shapes.Add(Square);
        //shapes.Add(T_Block);
    }
}
