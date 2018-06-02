using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrominoManager : MonoBehaviour {
    
    public List<Tetronimo> shapes;

    public Tetronimo getRandomBlock(){

        // Random.Range does not include last value (so from 0 - shapes.Count - 1)
        return shapes[Random.Range(0, shapes.Count)];
    }  
}
