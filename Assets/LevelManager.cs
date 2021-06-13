using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LevelManager : MonoBehaviour
{
    public List<SceneReference> LevelsInOrder;

    public GameObject LevelCompleteScreenPanel;
    public GameObject PausePanel;

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
        PausePanel.SetActive(false);
        LevelCompleteScreenPanel.SetActive(false);
        var unload = UnloadCurrentLevel();
        unload.completed += LoadNextLevel;
    }

    private void Start()
    {
        PausePanel.SetActive(false);
        LevelCompleteScreenPanel.SetActive(false);

        _singleSceneMode = SceneManager.sceneCount > 1;

        if (!_singleSceneMode)
        {
            LoadCurrentLevel();
        }
        else
        {
            LoadOperationOnCompleted(null);
        }
    }

    private bool _paused;

    private void Update()
    {
        if (Input.GetButtonDown("Pause"))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        _paused = !_paused;

        Time.timeScale = _paused ? 0f : 1f;

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

    private void LoadCurrentLevel()
    {
        Time.timeScale = 1;
        var level         = LevelsInOrder[_currentLevel];
        var loadOperation = SceneManager.LoadSceneAsync(level.ScenePath, LoadSceneMode.Additive);

        loadOperation.allowSceneActivation = true;

        loadOperation.completed += LoadOperationOnCompleted;
    }

    private void LoadOperationOnCompleted(AsyncOperation obj)
    {
        var goal = FindObjectOfType<LevelGoal>();
        if (goal != null)
            goal.OnGoalReached += GoalOnOnGoalReached;
    }


    private void GoalOnOnGoalReached(object sender, EventArgs e)
    {
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
        if (_currentLevel > LevelsInOrder.Count)
        {
            _currentLevel = 0;
        }

        LoadCurrentLevel();
    }
}
