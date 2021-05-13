using System.Collections;
using System.Collections.Generic;
//using System;
using UnityEngine;

public class ConstructorMaze : MonoBehaviour
{
    public GameObject prefabWoll;
    public GameObject prefabFloor;
    public GameObject prefabTop;
    public GameObject prefabGold;
    public GameObject player;
    public GameObject exit;
    public GameObject prefabKeys;
    public GameObject prefabMonster1;
    public GameObject prefabMonster2;
    
    public int sizeCell = 2;
    public int amountGold;
    public int amountKeys;
    public int amountMonster1;
    public int amountMonster2;

    private MazeDataGenerator dataGenerator;
    private int _randomX, _randomZ;
    private List<GameObject> listPrefabs;
    private List<GameObject> listGold;
    private int sizeList;
    
    

    public int[,] Data
    {
        get; private set;
    }
    public int[,] CopyData
    {
        get; private set;
    }
    
    public int PositionX
    {
        get; private set;
    }
    public int PositionZ
    {
        get; private set;
    }

    private void Awake()
    {
        dataGenerator = new MazeDataGenerator();
        

        Data = new int[,]
        {
            {1, 1, 1},
            {1, 0, 1},
            {1, 1, 1}
        };
    }

    void AddArray (int size, GameObject prefab)
    {
        for (int i = 0; i < size; i++)
        {
            listPrefabs.Add(prefab);
        }
    }
    public void GenerateNewMaze(int sizeRows, int sizeCols)
    {
        if (sizeRows % 2 == 0 && sizeCols % 2 == 0)
        {
            Debug.LogError("Odd numbers work better for dungeon size.");
        }

        Data = dataGenerator.FromDimensions(sizeRows, sizeCols);
        CopyData = Data;

        GeneratorLists();  // генерируем списки 

        DisplayMaze(sizeRows, sizeCols, Data);      // отображаем лабиринт
        FindRandomPosition(sizeList, listPrefabs);  // раскидываем игрока, ключи, выход и монстров
        FindRandomPosition(amountGold, listGold);   // раскидываем золото        
                
    }

    private void DisplayMaze(int sizeRows, int sizeCols, int[,] data)
    {
        for (int i = 0; i < sizeRows; i++)
        {
            for (int j = 0; j < sizeCols; j++)
            {
                if (CopyData[i, j] == 1)
                {
                    GameObject go = Instantiate<GameObject>(prefabWoll);
                    go.transform.position = new Vector3(i*sizeCell, 0, j*sizeCell);
                }
                
            }
        }

        GameObject floor = Instantiate<GameObject>(prefabFloor);
        floor.transform.position = new Vector3(sizeRows * sizeCell / 2, -1, sizeCols * sizeCell / 2);
        floor.transform.localScale = new Vector3(sizeRows / 3, 1, sizeCols / 3);

        GameObject top = Instantiate<GameObject>(prefabTop);
        top.transform.position = new Vector3(sizeRows * sizeCell / 2, 1, sizeCols * sizeCell / 2);
        top.transform.localScale = new Vector3(sizeRows / 3, -1, sizeCols / 3);
    }


    /*  
     *  помещение цели в центре лабиринта
     *  
    private void FindPlayerPosition()
    {
        
        for (int i = data.GetLength(0) / 2 - 2; i < data.GetLength(0) / 2 + 2; i++)
        {
            for (int j = data.GetLength(1) / 2 - 2; j < data.GetLength(1) / 2 + 2; j++)
            {
                if (data[i, j] == 0)
                {
                    startRow = i*sizeCell;
                    startCol = j*sizeCell;
                    return;
                }
            }
        }
    }
    */


    private void FindRandomPosition(int amount, List<GameObject> gameObjects)
    {
        for (int a = 0; a < amount; a++)
        {
            _randomX = 0;
            _randomZ = 0;
            _randomX = Random.Range(1, Data.GetLength(0) - 2);
            _randomZ = Random.Range(1, Data.GetLength(1) - 2);
            if (CopyData[_randomX, _randomZ] == 0)
            {
                PositionX = _randomX * sizeCell;
                PositionZ = _randomZ * sizeCell;
            }
            else 
            {
                for (int i = _randomX - 1; i <= _randomX + 1; i++)
                {
                    for (int j = _randomZ - 1; j <= _randomZ + 1; j++)
                    {
                        if (CopyData[i, j] == 0)
                        {
                            PositionX = i * sizeCell;
                            PositionZ = j * sizeCell;
                            _randomX = i;
                            _randomZ = j;
                        }
                    }
                }
            }
            GameObject go = Instantiate<GameObject>(gameObjects[a]);
            go.transform.position = new Vector3(PositionX, 0, PositionZ);
            CopyData[_randomX, _randomZ] = 1;
        }
        
                
    }
    
    private void GeneratorLists ()
    {

        listPrefabs = new List<GameObject> { player, exit };
        AddArray(amountMonster1, prefabMonster1);
        AddArray(amountMonster2, prefabMonster2);
        AddArray(amountKeys, prefabKeys);
        sizeList = listPrefabs.Count;

        // перемешываем список 4 раза
        for (int j = 0; j < 4; j++)
        {
            for (int i = 0; i < listPrefabs.Count; i++)
            {
                GameObject tmp = listPrefabs[0];
                listPrefabs.RemoveAt(0);
                listPrefabs.Insert(Random.Range(0, listPrefabs.Count), tmp);
            }
        }
        
        listGold = new List<GameObject>();
        for (int i = 0; i < amountGold; i++)
        {
            listGold.Add(prefabGold);
        }
    }
    
}
