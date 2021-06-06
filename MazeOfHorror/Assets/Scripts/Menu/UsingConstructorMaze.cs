using System.Collections.Generic;
using UnityEngine;
public class UsingConstructorMaze : MonoBehaviour
{
    public SliderRowCol sizeRows;
    public SliderRowCol sizeCels;
    public SliderRowCol amountKeys;
    public SliderRowCol amountMonster;
    public SliderRowCol amountMonster1;
    public SliderRowCol amountMonster2;

    private List<int> constMaze;

    public List<int> UsingConstructor()
    {
        constMaze = new List<int>();
        constMaze.Add(sizeRows._size);
        constMaze.Add(sizeCels._size);
        constMaze.Add(amountKeys._size);
        constMaze.Add(amountMonster._size);
        constMaze.Add(amountMonster1._size);
        constMaze.Add(amountMonster2._size);

        return constMaze;
    }
}
