using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    private LevelManager _levelManager;

    // Start is called before the first frame update
    void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();

        _levelManager.AllowPause = false;
    }

    [UsedImplicitly]
    public void StartGame()
    {
        _levelManager.LoadNextLevel();
    }

    [UsedImplicitly]
    public void QuitGame()
    {
        Application.Quit();
    }
}
