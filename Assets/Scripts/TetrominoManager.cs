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
    public List<Color> colors = new List<Color>();

    public Tetromino getRandomBlock(){
        /* Note: Random.Range does not include last value (so from 0 - shapes.Count - 1)
         * 
         * pick random num. this num will decide the shape it picks from List shapes,
         * and it will also pick the same number color from List colors. That means
         * that each shape type has a unique color to it.
        */
        int num = Random.Range(0, shapes.Count);

        Tetromino block = new Tetromino(shapes[num].points[0],shapes[num].points[1],shapes[num].points[2]);

        // set block color using same random num
        block.BlockColor = colors[num];

        return block;
    }

    // Randomizes color list so blocks get new default color each game.
    void RandomizeColors() {
        for (int i = 0; i < shapes.Count; i++){
            colors.Add(new Color(Random.Range(0.1f, 0.8f), Random.Range(0.1f, 0.8f), Random.Range(0.1f, 0.8f)));
        }
    }

    void Start()
    {
        shapes.Add(I_Block);
        //shapes.Add(L_Block);
        //shapes.Add(J_Block);
        shapes.Add(Plus);
        //shapes.Add(Z_Block);
        //shapes.Add(S_Block);
        shapes.Add(Square);
        //shapes.Add(T_Block);

        RandomizeColors();
    }
}
