using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControler : MonoBehaviour
{

    private ConstructorMaze generator;
    private bool startGame = false;

    private int lifes;
    private int keys, startKeys;
    private int gold, startGold;
    private int cubes;
    private float gameTime;
    private int seconds;
    private int minutes;

    public MenuController menuContr;
    public Text textLifes;
    public Text textKeys;
    public Text textGold;
    public Text textCubes;
    public Text textTimes;
    public Text textMessage;
    public Text resultLife;
    public Text resultGold;
    public Text resultTime;
    public Text resultLifeLose;
    public Text resultGoldLose;
    public Text resultTimeLose;
    public Image minusLife;
    public Image cursorHandler;
    public Image cube;

    [HideInInspector]
    public bool cursorOnOff = false;
    public bool cubeMovet = false;


    public bool pausedGame { get; private set; }

    void Start()
    {
        generator = GetComponent<ConstructorMaze>();

    }


    public void StartNewGame(List<int> constrMaze)
    {
        generator.GenerateNewMaze(constrMaze);
        startKeys = constrMaze[2];
        startGold = (constrMaze[0] + constrMaze[1]) / 2;
        lifes = startGold / 5;
        cubes = lifes;
        keys = startKeys;
        gold = startGold;
        seconds = 0;
        minutes = 0;

        startGame = true;
        pausedGame = false;
    }

    public void PausedGame(bool chek)
    {
        pausedGame = chek;
    }

    public void RestartGame()
    {
        seconds = 0;
        minutes = 0;
        lifes = startGold / 5;
        keys = startKeys;
        gold = startGold;
        generator.Restart();
    }

    private void OnGUI()
    {
        if (startGame)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (cursorOnOff)
            {
                cursorHandler.enabled = true;
            }
            if (!cursorOnOff)
            {
                cursorHandler.enabled = false;
            }
            if (cubeMovet)
            {
                cube.enabled = true;
            }
            if (!cubeMovet)
            {
                cube.enabled = false;
            }
        }

        if (pausedGame)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        textLifes.text = $"Life: {lifes}";
        textKeys.text = $"Keys: {keys} / {startKeys}";
        textGold.text = $"Gold: {gold} / {startGold}";
        textCubes.text = $"{cubes}";
        textTimes.text = $"{minutes:d2} : {seconds:d2}";

    }
    public void ZeroAllCell()
    {
        GameObject[] allCell = GameObject.FindGameObjectsWithTag("Cell");
        foreach (var e in allCell)
        {
            e.GetComponentInChildren<CellMovet>().pressKeyF = false;
            e.GetComponentInChildren<CellMovet>().pressKeyMouse = false;
        }
    }

    public void Message(string newText)
    {
        textMessage.text = newText;
    }

    public void MinusLifes()
    {
        lifes--;
        if (lifes < 0) lifes = 0;
        if (lifes == 0) PlayerLose();
        pausedGame = true;
        generator.TransformingMonstersAffterMinusLifePlayer();
        Animation anim = minusLife.GetComponent<Animation>();
        anim.Play();

    }

    public void MinusKeys()
    {
        keys--;
        if (keys < 0) keys = 0;
        if (keys == 0)
        {
            ParticleSystem part = GameObject.Find("PrefabExit(Clone)").GetComponentInChildren<ParticleSystem>();
            ParticleSystem.MainModule main = part.main;
            main.startColor = Color.green;
        }
    }

    public int GetKeys()
    {
        return keys;
    }

    public void MinusCube()
    {
        cubes--;
        if (cubes < 0) cubes = 0;

    }

    public int GetCube()
    {
        return cubes;
    }

    public void MinusGold()
    {
        gold--;
        if (gold < 0) gold = 0;
    }

    public void PlayerWin()
    {

        pausedGame = true;
        menuContr.Victory();
        resultGold.text = $"Remaining to collect gold {gold} / {startGold}";
        resultLife.text = $"Remaining lives {lifes} / {startGold / 5}";
        resultTime.text = $"Wasted time {minutes:d2} : {seconds:d2}";
    }

    public void PlayerLose()
    {
        pausedGame = true;
        menuContr.Luser();
        resultGold.text = $"Remaining to collect gold {gold} / {startGold}";
        resultLife.text = $"Remaining lives {lifes} / {startGold / 5}";
        resultTime.text = $"Wasted time {minutes:d2} : {seconds:d2}";
    }


    void Update()
    {

        if (!pausedGame)
        {
            gameTime += 1 * Time.deltaTime;
            if (gameTime >= 1)
            {
                seconds += 1;
                gameTime = 0;
            }
            if (seconds >= 60)
            {
                minutes += 1;
                seconds = 0;
            }
        }
    }
}
