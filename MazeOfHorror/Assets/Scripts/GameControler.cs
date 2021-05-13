using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControler : MonoBehaviour
{
    //[SerializeField] private FpsMovement player;
    private ConstructorMaze generator;
    public int sizeRows;
    public int sizeCols;

    void Start()
    {
        generator = GetComponent<ConstructorMaze>();
        StartNewGame();
    
    }

    void StartNewGame ()
    {
        generator.GenerateNewMaze(sizeRows, sizeCols);
        
    }
    
    void Update()
    {
        
    }
}
