using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;

public class PlayerPrefManager : MonoBehaviour
{
    public const string MusicVolumePref    = "MusicVolume";
    public const string BloomIntensityPref = "BloomIntensity";

    public  PostProcessProfile PostProcess;
    public  AudioMixer         AudioMixer;
    private float              bloomValue;
    private float              musicVolumeValue;

    [ShowNativeProperty]
    public float BloomValue
    {
        get => bloomValue;
        set
        {
            bloomValue                                      = value;
            PostProcess.GetSetting<Bloom>().intensity.value = value;
        }
    }

    [ShowNativeProperty]
    public float MusicVolumeValue
    {
        get => musicVolumeValue;
        set
        {
            musicVolumeValue = value;
            AudioMixer.SetFloat(MusicVolumePref, Mathf.Log10(value) * 20);
        }
    }

    private void Start()
    {
        LoadSettings();
    }

    public void LoadSettings()
    {
        BloomValue       = PlayerPrefs.GetFloat(BloomIntensityPref, 6);
        MusicVolumeValue = PlayerPrefs.GetFloat(MusicVolumePref, 0.8f);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(MusicVolumePref, MusicVolumeValue);
        PlayerPrefs.SetFloat(BloomIntensityPref, BloomValue);
    }
}
