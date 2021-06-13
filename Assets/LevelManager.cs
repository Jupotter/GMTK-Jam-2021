using System;
using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;
#if UNITY_EDITOR
using UnityEditor;

#endif

public class LevelManager : MonoBehaviour
{
    public SceneReference MainMenu;

    public List<SceneReference> LevelsInOrder;

    public GameObject LevelCompleteScreenPanel;
    public GameObject PausePanel;
    public GameObject CompleteGamePanel;

    public GameObject LevelTimeDisplay;

    public TimeSpan TotalTime { get; private set; }
    public TimeSpan LevelTime => _stopwatch.Elapsed;

    private readonly Stopwatch _stopwatch = new Stopwatch();

    public bool AllowPause { get; set; }

    [ShowNonSerializedField] private int _currentLevel;

    [ShowNonSerializedField] private bool _singleSceneMode;


    [UsedImplicitly]
    public void RetryLevel()
    {
        PausePanel.SetActive(false);
        LevelCompleteScreenPanel.SetActive(false);
        var unload = UnloadCurrentLevel();
        unload.completed += op => LoadCurrentLevel();
    }

    [UsedImplicitly]
    public void LoadNextLevel()
    {
        TotalTime += LevelTime;
        PausePanel.SetActive(false);
        LevelCompleteScreenPanel.SetActive(false);

        var            level = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        AsyncOperation unload;
        if (level.path == MainMenu.ScenePath)
        {
            unload           =  SceneManager.UnloadSceneAsync(MainMenu.ScenePath);
            unload.completed += LoadCurrentLevel;
        }
        else
        {
            unload           =  UnloadCurrentLevel();
            unload.completed += LoadNextLevel;
        }
    }

    private void Start()
    {
        CompleteGamePanel.SetActive(false);
        PausePanel.SetActive(false);
        LevelCompleteScreenPanel.SetActive(false);

        _singleSceneMode = SceneManager.sceneCount > 1;

        if (!_singleSceneMode)
        {
            LoadMainMenu();
        }
        else
        {
            _currentLevel = SceneManager.GetSceneAt(1).buildIndex - 1;
            LoadOperationOnCompleted(null);
        }
    }

    public void LoadMainMenu()
    {
        LevelTimeDisplay.SetActive(false);
        if (SceneManager.sceneCount > 1)
            SceneManager.LoadScene("Level Manager", LoadSceneMode.Single);

        SceneManager.LoadScene(MainMenu.ScenePath, LoadSceneMode.Additive);
    }

    private bool _paused;

    private void Update()
    {
        if (Input.GetButtonDown("Pause") && AllowPause)
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        _paused = !_paused;

        Time.timeScale = _paused ? 0f : 1f;

        if (_paused)
            _stopwatch.Stop();
        else
            _stopwatch.Start();
        PausePanel.SetActive(_paused);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void LoadCurrentLevel(AsyncOperation obj) => LoadCurrentLevel();

    private void LoadCurrentLevel()
    {
        AllowPause = true;
        LevelTimeDisplay.SetActive(true);
        Time.timeScale = 1;
        var level         = LevelsInOrder[_currentLevel];
        var loadOperation = SceneManager.LoadSceneAsync(level.ScenePath, LoadSceneMode.Additive);

        loadOperation.allowSceneActivation = true;

        loadOperation.completed += LoadOperationOnCompleted;
    }

    private void LoadOperationOnCompleted(AsyncOperation obj)
    {
        _stopwatch.Restart();
        var goal = FindObjectOfType<LevelGoal>();
        if (goal != null)
            goal.OnGoalReached += GoalOnOnGoalReached;
    }


    private void GoalOnOnGoalReached(object sender, EventArgs e)
    {
        AllowPause = false;
        _stopwatch.Stop();
        Time.timeScale = 0;
        LevelCompleteScreenPanel.SetActive(true);
    }

    private AsyncOperation UnloadCurrentLevel()
    {
        var level = LevelsInOrder[_currentLevel];
        return SceneManager.UnloadSceneAsync(level.ScenePath);
    }


    private void LoadNextLevel(AsyncOperation obj)
    {
        AllowPause = true;
        if (_singleSceneMode)
        {
            LoadCurrentLevel();
            return;
        }

        _currentLevel++;
        if (_currentLevel >= LevelsInOrder.Count)
        {
            LoadEndScreen();
            return;
        }

        LoadCurrentLevel();
    }

    private void LoadEndScreen()
    {
        LevelTimeDisplay.SetActive(false);
        CompleteGamePanel.SetActive(true);
    }
}
