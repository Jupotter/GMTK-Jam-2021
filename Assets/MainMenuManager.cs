using JetBrains.Annotations;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class MainMenuManager : MonoBehaviour
{
    public SettingsManager SettingsMenu;

    private LevelManager _levelManager;

    // Start is called before the first frame update
    void Start()
    {
        _levelManager = FindObjectOfType<LevelManager>();

        _levelManager.AllowPause = false;

        SettingsMenu.gameObject.SetActive(false);
    }

    [UsedImplicitly]
    public void StartGame()
    {
        _levelManager.LoadNextLevel();
    }

    [UsedImplicitly]
    public void OpenSettingsMenu()
    {
        SettingsMenu.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    [UsedImplicitly]
    public void QuitGame()
    {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
