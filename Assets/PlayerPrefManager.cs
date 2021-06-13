using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.PostProcessing;

public class PlayerPrefManager : MonoBehaviour
{
    public const string MusicVolumePref    = "MusicVolume";
    public const string SfxVolumePref      = "SfxVolume";
    public const string BloomIntensityPref = "BloomIntensity";

    public PostProcessProfile PostProcess;
    public AudioMixer         AudioMixer;

    private float bloomValue;
    private float musicVolume;
    private float sfxVolume;

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
    public float MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = value;
            AudioMixer.SetFloat(MusicVolumePref, Mathf.Log10(value) * 20);
        }
    }

    [ShowNativeProperty]
    public float SfxVolume
    {
        get => sfxVolume;
        set
        {
            sfxVolume = value;
            AudioMixer.SetFloat(SfxVolumePref, Mathf.Log10(value) * 20);
        }
    }

    private void Start()
    {
        LoadSettings();
    }

    public void LoadSettings()
    {
        BloomValue  = PlayerPrefs.GetFloat(BloomIntensityPref, 6);
        MusicVolume = PlayerPrefs.GetFloat(MusicVolumePref, 0.8f);
        SfxVolume   = PlayerPrefs.GetFloat(SfxVolumePref, 0.8f);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(MusicVolumePref, MusicVolume);
        PlayerPrefs.SetFloat(SfxVolumePref, SfxVolume);
        PlayerPrefs.SetFloat(BloomIntensityPref, BloomValue);
    }
}
