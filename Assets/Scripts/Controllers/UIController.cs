using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : BaseController, IExecute
{
    private MainMenuView _mainMenu;
    private InGameView _inGame;
    private PauseMenuView _pauseMenu;
    private EndGameMenuView _endGameMenu;
    private ShopMenuView _shopMenu;

    public UIController(MainController main) : base(main) 
    {
        UIEvents.Current.OnButtonStartGame += StartGame;
        UIEvents.Current.OnButtonPauseGame += PauseGame;
        UIEvents.Current.OnButtonResumeGame += ResumeGame;
        UIEvents.Current.OnButtonRestartGame += RestartGame;

        GameEvents.current.OnLevelComplete += WinGame;
        GameEvents.current.OnLevelFailed += LoseGame;
    }

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Execute()
    {
        base.Execute();
        //TODO
        //Update money on UI
    }

    private void StartGame()
    {
        SwitchUI(UIState.InGame);
        GameEvents.current.LevelStart();
    }
    private void PauseGame()
    {
        SwitchUI(UIState.Pause);
        GameEvents.current.GamePaused();
    }
    private void ResumeGame()
    {
        SwitchUI(UIState.InGame);
        GameEvents.current.GameResumed();
    }
    private void WinGame()
    {
        SwitchUI(UIState.EndGame);
        _endGameMenu.ActivateState(true);
    }
    private void LoseGame()
    {
        SwitchUI(UIState.EndGame);
        _endGameMenu.ActivateState(false);
    }
    private void RestartGame()
    {
        GameEvents.current.LevelRestart();
    }

    public void AddView(MainMenuView view)
    {
        _mainMenu = view;
    }
    public void AddView(InGameView view)
    {
        _inGame = view;
    }
    public void AddView(PauseMenuView view)
    {
        _pauseMenu = view;
    }
    public void AddView(EndGameMenuView view)
    {
        _endGameMenu = view;
    }
    public void AddView(ShopMenuView view)
    {
        _shopMenu = view;
    }

    private void SwitchUI(UIState state)
    {
        switch (state)
        {
            case UIState.MainMenu:
                _mainMenu.Show();
                _inGame.Hide();
                _pauseMenu.Hide();
                _endGameMenu.Hide();
                _shopMenu.Hide();
                break;
            case UIState.InGame:
                _mainMenu.Hide();
                _inGame.Show();
                _pauseMenu.Hide();
                _endGameMenu.Hide();
                _shopMenu.Hide();
                break;
            case UIState.Pause:
                _mainMenu.Hide();
                _inGame.Hide();
                _pauseMenu.Show();
                _endGameMenu.Hide();
                _shopMenu.Hide();
                break;
            case UIState.EndGame:
                _mainMenu.Hide();
                _inGame.Hide();
                _pauseMenu.Hide();
                _endGameMenu.Show();
                _shopMenu.Hide();
                break;
            case UIState.Shop:
                _mainMenu.Hide();
                _inGame.Hide();
                _pauseMenu.Hide();
                _endGameMenu.Hide();
                _shopMenu.Show();
                break;
        }
    }
}