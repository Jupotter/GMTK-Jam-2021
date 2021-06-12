using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public List<SceneAsset> LevelsInOrder;

    [ShowNonSerializedField] private bool _singleSceneMode;
    [ShowNonSerializedField] private int  _currentLevel;

    // Start is called before the first frame update
    void Start()
    {
        _singleSceneMode = SceneManager.sceneCount > 1;

        if (!_singleSceneMode)
            LoadCurrentLevel();
        else
            LoadOperationOnCompleted(null);
    }

    private void LoadCurrentLevel()
    {
        var level         = LevelsInOrder[_currentLevel];
        var loadOperation = SceneManager.LoadSceneAsync(level.name, LoadSceneMode.Additive);

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
        var level  = LevelsInOrder[_currentLevel];
        var unload = SceneManager.UnloadSceneAsync(level.name);
        unload.completed += UnloadOnCompleted;
    }

    private void UnloadOnCompleted(AsyncOperation obj)
    {
        if (_singleSceneMode)
        {
            LoadCurrentLevel();
            return;
        }

        _currentLevel++;
        if (_currentLevel > LevelsInOrder.Count)
            _currentLevel = 0;
        LoadCurrentLevel();
    }
}
