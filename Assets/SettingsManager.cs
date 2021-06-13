using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public MainMenuManager MainMenu;

    public Slider MusicVolumeSlider;
    public Slider BloomSlider;

    public PostProcessProfile PostProcess;
    public AudioMixer         AudioMixer;

    private PlayerPrefManager _playerPrefManager;

    private void Start()
    {
        _playerPrefManager = FindObjectOfType<PlayerPrefManager>();
        Debug.Assert(_playerPrefManager != null);

        ResetSliders();
    }

    private void ResetSliders()
    {
        MusicVolumeSlider.value = _playerPrefManager.MusicVolumeValue;
        BloomSlider.value       = _playerPrefManager.BloomValue;
    }

    [UsedImplicitly]
    public void OnMusicVolumeUpdated(float sliderValue)
    {
        _playerPrefManager.MusicVolumeValue = sliderValue;
    }

    [UsedImplicitly]
    public void OnBloomUpdated(float sliderValue)
    {
        _playerPrefManager.BloomValue = sliderValue;
    }

    [UsedImplicitly]
    public void OnApply()
    {
        _playerPrefManager.SaveSettings();

        ShowMainMenu();
    }

    [UsedImplicitly]
    public void OnCancel()
    {
        _playerPrefManager.LoadSettings();
        ResetSliders();

        ShowMainMenu();
    }

    private void ShowMainMenu()
    {
        MainMenu.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
