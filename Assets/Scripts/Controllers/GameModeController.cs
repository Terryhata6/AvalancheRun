using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GameAnalyticsSDK;
public class GameModeController : BaseController, IExecute
{
    #region Fields    
    private GameMode _gameMode;
    private SaveDataRepo _saveData;

    private string _levelNamePrefix = "Level";
    private int _maxLevelIndex = 10;
    private int _curretLevelIndex;
    private int _completedLevelValue;
    #endregion

    #region Acсess Modifyers
    public GameMode GameMode => _gameMode;
    #endregion

    private Dictionary<string, bool> _scenesList = new Dictionary<string, bool>();
    private Dictionary<string, bool> _scenesListTemp = new Dictionary<string, bool>();
    public GameModeController(MainController main) : base(main)
    {
        _main = main;
        _saveData = new SaveDataRepo();
        _curretLevelIndex = _saveData.LoadInt(SaveKeyManager.LevelNumber);
        _completedLevelValue = _saveData.LoadInt(SaveKeyManager.ComplitedLevelValue);
    }

    public override void Initialize()
    {
        base.Initialize();

        GameEvents.Current.OnLevelStart += StartGame;
        GameEvents.Current.OnGamePaused += PauseGame;
        GameEvents.Current.OnGameResumed += ResumeGame;
        GameEvents.Current.OnLevelComplete += LevelComplete;
        GameEvents.Current.OnNextLevel += LoadLevel;
        GameEvents.Current.OnLevelRestart += LoadLevel;
    }

    public override void Execute()
    {
        base.Execute();
        if (!IsActive)
        {
            return;
        }
    }

    private void LoadAsyncScene(string sceneName)
    {
        if (_scenesList.ContainsKey(sceneName))
        {
            if (_scenesList[sceneName] == false)
            {
                SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
                Debug.Log($"Scene {sceneName} was loaded");
                _scenesList[sceneName] = true;
            }
            else
            {
                Debug.LogError("Scene already loaded and can't be loaded again");
            }
        }
        else
        {
            AddSceneToList(sceneName);
            LoadAsyncScene(sceneName);
        }
    }
    
    private void UnloadAsyncScene(string sceneName)
    {
        if (_scenesList.ContainsKey(sceneName))
        {
            if (_scenesList[sceneName])
            {
                SceneManager.UnloadSceneAsync(sceneName);
                Debug.Log($"Scene {sceneName} was unloaded");
                _scenesList[sceneName] = false;
            }
        }
        
    }
    /// <summary>
    /// Add new scene in scene name
    /// </summary>
    /// <param name="sceneName"></param>
    private void AddSceneToList(string sceneName)
    {
        _scenesList.Add(sceneName, false);
        Debug.Log($"Scene {sceneName} was added in sceneList");
    }
    /// <summary>
    /// unload all loaded scenes from list and load sceneName scene
    /// </summary>
    /// <param name="sceneName"></param>
    public void LoadNewScene(string sceneName)
    {
        _scenesListTemp.Clear();
        foreach (KeyValuePair<string, bool> entry in _scenesList)
        {
            _scenesListTemp.Add(entry.Key, entry.Value);
        }
        foreach (KeyValuePair<string, bool> entry in _scenesListTemp)
        {       
            if (entry.Value)
            {
                UnloadAsyncScene(entry.Key);                
            }
        }
        LoadAsyncScene(sceneName);
        GameEvents.Current.SetActiveCamera("Up-to-up Virtual Camera");
        GameEvents.Current.SceneChanged();
        GameEvents.Current.PlayerControllerSetActive(false);
        
    }

    public void LoadLevel()
    {        
        LoadLevel($"{_levelNamePrefix}{_curretLevelIndex}");
    }

    public void LoadLevel(string levelName)
    {
        LoadNewScene(levelName);
    }

    private void LevelComplete()
    {
        _curretLevelIndex++;
        _completedLevelValue++;
        if (_curretLevelIndex > _maxLevelIndex)
        {
            _main.IsDermische = true;
            _curretLevelIndex = 0;
        }
        Debug.LogWarning($"{_completedLevelValue}");
        _saveData.SaveData(_curretLevelIndex, SaveKeyManager.LevelNumber);
        _saveData.SaveData(_completedLevelValue, SaveKeyManager.ComplitedLevelValue);
        GameAnalytics.NewDesignEvent($"LevelsCompleted: {_completedLevelValue}");
    }

    private void StartGame()
    {
        Time.timeScale = 1f;
    }
    private void PauseGame()
    {
        Time.timeScale = 0;
    }
    private void ResumeGame()
    {
        Time.timeScale = 1f;
    }
}