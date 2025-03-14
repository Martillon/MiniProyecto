using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    #region Variables
    
    [Header("Audio")]
    public AudioMixer audioMixer;
    public Slider masterVolumeSlider;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    
    public string masterVolumeParameter = "MasterVolume";
    public string musicVolumeParameter = "MusicVolume";
    public string sfxVolumeParameter = "SFXVolume";
    
    [Header("Video")]
    public Dropdown resolutionDropdown;
    public Toggle fullscreenToggle;

    private Resolution[] resolutions;
    
    #endregion
    
    private void Awake()
    {
        masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSfxVolume);
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    private void Start()
    {
        masterVolumeSlider.value = PlayerPrefs.GetFloat(SettingsManager.MasterVolume,1f);
        sfxVolumeSlider.value = PlayerPrefs.GetFloat(SettingsManager.SfxVolume, 1f);
        musicVolumeSlider.value = PlayerPrefs.GetFloat(SettingsManager.MusicVolume, 1f);
        
        // Load resolutions
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Assign event
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        
        // Load fullscreen
        fullscreenToggle.isOn = PlayerPrefs.GetInt("Fullscreen", 1) == 1;
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }
    
    #region Audio

    void SetMasterVolume(float volume)
    {
        audioMixer.SetFloat(masterVolumeParameter, Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat(SettingsManager.MasterVolume, volume);
    }

    void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat(musicVolumeParameter, Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat(SettingsManager.MusicVolume, volume);
    }

    void SetSfxVolume(float volume)
    {
        audioMixer.SetFloat(sfxVolumeParameter, Mathf.Log10(Mathf.Max(volume, 0.0001f)) * 20);
        PlayerPrefs.SetFloat(SettingsManager.SfxVolume, volume);
    }
    
    #endregion

    #region Video

    void SetResolution(int index)
    {
        Screen.SetResolution(resolutions[index].width, resolutions[index].height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", index);
    }
    void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("Fullscreen", isFullscreen ? 1 : 0);
    }
    
    #endregion
    
}
