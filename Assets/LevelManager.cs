using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<SceneReference> LevelsInOrder;

    public GameObject LevelCompleteScreenPanel;

    [ShowNonSerializedField] private int _currentLevel;

    [ShowNonSerializedField] private bool _singleSceneMode;


    [UsedImplicitly]
    public void RetryLevel()
    {
        LevelCompleteScreenPanel.SetActive(false);
        var unload = UnloadCurrentLevel();
        unload.completed += op => LoadCurrentLevel();
    }

    [UsedImplicitly]
    public void LoadNextLevel()
    {
        LevelCompleteScreenPanel.SetActive(false);
        var unload = UnloadCurrentLevel();
        unload.completed += LoadNextLevel;
    }

    private void Start()
    {
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
