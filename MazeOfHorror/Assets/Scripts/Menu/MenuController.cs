using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    enum Screen
    {
        Game,
        Global,
        Setting,
        GameChange,
        EasyMediumHard,
        Paused,
        SettingInPause,
        Constructor,
        Victory,
        Loser
    }
    private Screen screen;
    List<int> constrMaze;

    public GameControler gameContr;
    public UsingConstructorMaze usingConstr;
    public SaveLoadGame saveLoadGame;    

    public CanvasGroup gameCanvas;
    public CanvasGroup globalMenu;
    public CanvasGroup settingMenu;
    public CanvasGroup gameChangeMenu;
    public CanvasGroup easyMediumHardMenu;
    public CanvasGroup pausedMenu;
    public CanvasGroup settingInPause;
    public CanvasGroup constructor;
    public CanvasGroup victory;
    public CanvasGroup loser;

    void SetCurrentScreen(Screen screen)
    {
        Utility.SetCanvasGroupEnabled(gameCanvas, screen == Screen.Game);
        Utility.SetCanvasGroupEnabled(globalMenu, screen == Screen.Global);
        Utility.SetCanvasGroupEnabled(settingMenu, screen == Screen.Setting);
        Utility.SetCanvasGroupEnabled(gameChangeMenu, screen == Screen.GameChange);
        Utility.SetCanvasGroupEnabled(easyMediumHardMenu, screen == Screen.EasyMediumHard);
        Utility.SetCanvasGroupEnabled(pausedMenu, screen == Screen.Paused);
        Utility.SetCanvasGroupEnabled(settingInPause, screen == Screen.SettingInPause);
        Utility.SetCanvasGroupEnabled(constructor, screen == Screen.Constructor);
        Utility.SetCanvasGroupEnabled(victory, screen == Screen.Victory);
        Utility.SetCanvasGroupEnabled(loser, screen == Screen.Loser);
    }

    void Start()
    {

        saveLoadGame.Load();
        SetCurrentScreen(Screen.Global);
        screen = Screen.Global;
    }

    public void NewGame()
    {
        SetCurrentScreen(Screen.GameChange);
        screen = Screen.GameChange;
    }

    public void Setting()
    {
        SetCurrentScreen(Screen.Setting);
        screen = Screen.Setting;
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ExitSetting()
    {
        saveLoadGame.Save();
        SetCurrentScreen(Screen.Global);
        screen = Screen.Global;
    }

    public void StartRandomLevel()
    {
        SetCurrentScreen(Screen.EasyMediumHard);
        screen = Screen.EasyMediumHard;
    }

    public void ConstructorLevel()
    {
        SetCurrentScreen(Screen.Constructor);
        screen = Screen.Constructor;
    }

    public void Tutorial()
    {
        // В разработке
    }

    public void LoadLevel()
    {
        // В разработке
    }

    public void ExitGame()
    {
        SetCurrentScreen(Screen.Global);
        screen = Screen.Global;
    }

    public void Easy()
    {
        SetCurrentScreen(Screen.Game);
        screen = Screen.Game;
        constrMaze = new List<int> { 15, 15, 1, 1, 1, 1 };
        gameContr.StartNewGame(constrMaze);
    }

    public void Medium()
    {
        SetCurrentScreen(Screen.Game);
        screen = Screen.Game;
        constrMaze = new List<int> { 25, 25, 2, 2, 2, 2 };
        gameContr.StartNewGame(constrMaze);
    }

    public void Hard()
    {
        SetCurrentScreen(Screen.Game);
        screen = Screen.Game;
        constrMaze = new List<int> { 45, 45, 3, 3, 3, 3 };
        gameContr.StartNewGame(constrMaze);
    }

    public void ExitEMH()
    {
        SetCurrentScreen(Screen.GameChange);
        screen = Screen.GameChange;
    }

    public void Paused()
    {
        saveLoadGame.Save();
        SetCurrentScreen(Screen.Paused);
        screen = Screen.Paused;
        gameContr.PausedGame(true);

    }

    public void Continue()
    {
        SetCurrentScreen(Screen.Game);
        screen = Screen.Game;
        gameContr.PausedGame(false);
    }

    public void Restart()
    {
        SetCurrentScreen(Screen.Game);
        gameContr.RestartGame();
        screen = Screen.Game;
        gameContr.PausedGame(false);
    }

    public void SettingInPaused()
    {
        SetCurrentScreen(Screen.SettingInPause);
        screen = Screen.SettingInPause;
    }


    public void ExitInMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void StartConstructorGame()
    {
        constrMaze = usingConstr.UsingConstructor();
        SetCurrentScreen(Screen.Game);
        screen = Screen.Game;
        gameContr.StartNewGame(constrMaze);
    }

    public void Victory()
    {
        SetCurrentScreen(Screen.Victory);
        screen = Screen.Victory;
    }

    public void Luser()
    {
        SetCurrentScreen(Screen.Loser);
        screen = Screen.Loser;
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.P))
        {
            switch (screen)
            {
                case Screen.Game:
                    Paused();
                    break;
                case Screen.Global:
                    Exit();
                    break;
                case Screen.Setting:
                    ExitSetting();
                    break;
                case Screen.GameChange:
                    ExitGame();
                    break;
                case Screen.EasyMediumHard:
                    ExitEMH();
                    break;
                case Screen.Paused:
                    Continue();
                    break;
                case Screen.SettingInPause:
                    Paused();
                    break;
                case Screen.Constructor:
                    ExitEMH();
                    break;
                case Screen.Victory:
                    ExitInMenu();
                    break;
                case Screen.Loser:
                    ExitInMenu();
                    break;

            }
        }
    }
}
