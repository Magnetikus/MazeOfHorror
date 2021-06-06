using System.Collections.Generic;
using UnityEngine;

public class ConstructorMaze : MonoBehaviour
{
    public int changeRange;

    public GameObject prefabWoll;       //1   - цифра замещения в конструкторе лабиринта
    public GameObject prefabPoint;      //0
    public GameObject prefabFloor;
    public GameObject prefabTop;
    public GameObject prefabGold;       //8
    public GameObject player;           //3
    public GameObject exit;             //2
    public GameObject prefabKeys;       //4
    public GameObject prefabMonster;    //5
    public GameObject prefabMonster1;   //6
    public GameObject prefabMonster2;   //7

    // данные для получения
    private int sizeRows;
    private int sizeCols;
    private int amountKeys;
    private int amountMonster;
    private int amountMonster1;
    private int amountMonster2;

    //данные без изменения
    private int sizeCell = 2;
    private int amountGold;
    private int minDistance;
    private MazeDataGenerator dataGenerator;
    private Transform transformPlayer;
    private GameObject[] transformsMonsters;

    private int[] listMonster;

    private List<GameObject> listDeletedObject;

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

    public void GenerateNewMaze(List<int> constrMaze)
    {
        listDeletedObject = new List<GameObject>();
        //получение данных
        sizeRows = constrMaze[0];
        sizeCols = constrMaze[1];
        amountKeys = constrMaze[2];
        amountMonster = constrMaze[3];
        amountMonster1 = constrMaze[4];
        amountMonster2 = constrMaze[5];


        Data = dataGenerator.FromDimensions(sizeRows, sizeCols);                                //генерация лабиринта
        CopyData = Data;

        amountGold = (sizeRows + sizeCols) / 2;                                                 // определяем количество золота
        minDistance = amountGold / 2;

        GeneratorListMonster();                                                                 // генерируем список монстров

        int[] posExit = FindRandomPosition();
        CopyData[posExit[0], posExit[1]] = 2;                                                   //ставим выход
        int[] posPlayer = FindRandomPositionDistanceTarget(posExit, minDistance);
        CopyData[posPlayer[0], posPlayer[1]] = 3;                                               //ставим игрока
        for (int i = 0; i < amountKeys; i++)
        {
            int[] posKey = FindRandomPositionDistanceTarget(posExit, minDistance - 2);
            CopyData[posKey[0], posKey[1]] = 4;                                                 //раскидываем ключи
        }
        for (int i = 0; i < listMonster.Length; i++)
        {
            int[] posMonster = FindRandomPositionDistanceTarget(posPlayer, minDistance - 2);
            CopyData[posMonster[0], posMonster[1]] = listMonster[i];                            //ставим монстров
        }
        for (int i = 0; i < amountGold; i++)
        {
            int[] posGold = FindRandomPosition();
            CopyData[posGold[0], posGold[1]] = 8;                                               //раскидываем золото
        }

        DisplayMaze(CopyData);      // отображаем лабиринт

    }

    public void Restart()
    {
        foreach (var e in listDeletedObject) Destroy(e);
        listDeletedObject.Clear();
        DisplayMaze(CopyData);
    }

    private void DisplayMaze(int[,] data)
    {
        for (int i = 0; i < sizeRows; i++)
        {
            for (int j = 0; j < sizeCols; j++)
            {
                switch (data[i, j])
                {
                    case (0):
                        CreateObject(prefabPoint, i, j);
                        break;
                    case (1):
                        GameObject go = Instantiate(prefabWoll);
                        go.transform.position = new Vector3(i * sizeCell, 0, j * sizeCell);
                        if (i == 0 || j == 0 || i == sizeRows - 1 || j == sizeCols - 1)
                        {
                            go.tag = "NoMovet";
                            listDeletedObject.Add(go);
                            continue;
                        }
                        go.tag = "Movet";
                        listDeletedObject.Add(go);
                        break;
                    case (2):
                        CreateObject(prefabPoint, i, j);
                        CreateObject(exit, i, j);
                        break;
                    case (3):
                        CreateObject(prefabPoint, i, j);
                        CreateObject(player, i, j);
                        break;
                    case (4):
                        CreateObject(prefabPoint, i, j);
                        CreateObject(prefabKeys, i, j);
                        break;
                    case (5):
                        CreateObject(prefabPoint, i, j);
                        CreateObject(prefabMonster, i, j);
                        break;
                    case (6):
                        CreateObject(prefabPoint, i, j);
                        CreateObject(prefabMonster1, i, j);
                        break;
                    case (7):
                        CreateObject(prefabPoint, i, j);
                        CreateObject(prefabMonster2, i, j);
                        break;
                    case (8):
                        CreateObject(prefabPoint, i, j);
                        CreateObject(prefabGold, i, j);
                        break;
                }

            }
        }

        GameObject floor = Instantiate<GameObject>(prefabFloor);
        floor.transform.position = new Vector3(sizeRows * sizeCell / 2, -1, sizeCols * sizeCell / 2);
        floor.transform.localScale = new Vector3(sizeRows / 3, 1, sizeCols / 3);
        listDeletedObject.Add(floor);

        GameObject top = Instantiate<GameObject>(prefabTop);
        top.transform.position = new Vector3(sizeRows * sizeCell / 2, 1, sizeCols * sizeCell / 2);
        top.transform.localScale = new Vector3(sizeRows / 3, -1, sizeCols / 3);
        listDeletedObject.Add(top);
    }

    private void CreateObject(GameObject prefab, int i, int j)
    {
        GameObject go = Instantiate(prefab);
        go.transform.position = new Vector3(i * sizeCell, 0, j * sizeCell);
        listDeletedObject.Add(go);
    }

    private int[] FindRandomPosition()
    {
        int randomX, randomZ;
        int[] result = new int[2];
        randomX = Random.Range(1, Data.GetLength(0) - 2);
        randomZ = Random.Range(1, Data.GetLength(1) - 2);

        if (CopyData[randomX, randomZ] != 0)

        {
            for (int i = randomX - 1; i <= randomX + 1; i++)
            {
                for (int j = randomZ - 1; j <= randomZ + 1; j++)
                {
                    if (CopyData[i, j] == 0)
                    {
                        randomX = i;
                        randomZ = j;
                    }
                }
            }
        }

        result[0] = randomX;
        result[1] = randomZ;
        return result;
    }

    private int[] FindRandomPositionDistanceTarget(int[] target, float distance)
    {
        Vector3 positionTarget = new Vector3(target[0], 0, target[1]);
        float distReal;
        int[] position;
        do
        {
            position = FindRandomPosition();
            Vector3 positionNew = new Vector3(position[0], 0, position[1]);
            distReal = (positionNew - positionTarget).magnitude;
        }
        while (distReal < distance);
        return position;
    }

    private void GeneratorListMonster()
    {
        listMonster = new int[amountMonster + amountMonster1 + amountMonster2];
        for (int i = 0; i < amountMonster + amountMonster1 + amountMonster2; i++)
        {
            if (i < amountMonster) listMonster[i] = 5;
            else if (i >= amountMonster + amountMonster1) listMonster[i] = 7;
            else listMonster[i] = 6;
        }

    }

    public void TransformingMonstersAffterMinusLifePlayer()
    {
        transformPlayer = GameObject.Find("Player(Clone)").transform;
        int x = (int)transformPlayer.position.x;
        int z = (int)transformPlayer.position.z;
        int[] posPlayer = { x, z };
        transformsMonsters = GameObject.FindGameObjectsWithTag("Monster");
        foreach (var go in transformsMonsters)
        {
            if ((go.transform.position - transformPlayer.position).magnitude < 7)
            {
                int[] newPosition = FindRandomPositionDistanceTarget(posPlayer, 10f);
                Vector3 positionGO = go.transform.position;
                positionGO.x = newPosition[0];
                positionGO.z = newPosition[1];
                go.transform.position = positionGO;
            }
        }
    }

}
