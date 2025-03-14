using UnityEngine;
using UnityEngine.Audio;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance { get; private set; }

    [Header("Audio")]
    public AudioMixer audioMixer;

    public const string MasterVolume = "MasterVolume";
    public const string MusicVolume = "MusicVolume";
    public const string SfxVolume = "SFXVolume";

    [Header("Video")]
    public const string ResolutionIndex = "ResolutionIndex";
    public const string Fullscreen = "Fullscreen";

    private Resolution[] resolutions;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadSettings(); // Load settings
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Load settings
    private void LoadSettings()
    {
        LoadAudioSettings();
        LoadVideoSettings();
    }

    #region Audio
    private void LoadAudioSettings()
    {
        if (audioMixer == null)
        {
            Debug.LogError("AudioMixer not assigned.");
            return;
        }

        float masterVolume = PlayerPrefs.GetFloat(MasterVolume, 1f);
        float musicVolume = PlayerPrefs.GetFloat(MusicVolume, 1f);
        float sfxVolume = PlayerPrefs.GetFloat(SfxVolume, 1f);

        audioMixer.SetFloat(MasterVolume, Mathf.Log10(Mathf.Max(masterVolume, 0.0001f)) * 20);
        audioMixer.SetFloat(MusicVolume, Mathf.Log10(Mathf.Max(musicVolume, 0.0001f)) * 20);
        audioMixer.SetFloat(SfxVolume, Mathf.Log10(Mathf.Max(sfxVolume, 0.0001f)) * 20);
    }
    #endregion
    

    #region Video
    
    private void LoadVideoSettings()
    {
        resolutions = Screen.resolutions;

        // Resolution
        int resolutionIndex = PlayerPrefs.GetInt(ResolutionIndex, resolutions.Length - 1);
        if (resolutionIndex >= 0 && resolutionIndex < resolutions.Length)
        {
            SetResolution(resolutionIndex);
        }

        // Fullscreen
        bool isFullscreen = PlayerPrefs.GetInt(Fullscreen, 1) == 1;
        SetFullscreen(isFullscreen);
    }

    private void SetResolution(int index)
    {
        if (resolutions == null || index < 0 || index >= resolutions.Length) return;

        Resolution resolution = resolutions[index];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt(ResolutionIndex, index);
    }

    private void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt(Fullscreen, isFullscreen ? 1 : 0);
    }
    
    #endregion
}
