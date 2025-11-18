using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    [Header("Resolution Settings")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    public Toggle fullscreenToggle;

    [Header("Audio Settings")]
    public AudioMixer mainMixer;
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    [Header("Map Settings")]
    public TMP_InputField seedInputField;

    private void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        List<string> options = new List<string>();

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

        int savedIndex = PlayerPrefs.GetInt("resolutionIndex", currentResolutionIndex);
        resolutionDropdown.value = savedIndex;
        resolutionDropdown.RefreshShownValue();
        SetResolution(savedIndex);

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        fullscreenToggle.isOn = PlayerPrefs.GetInt("isFullscreen", 1) == 1;

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        // AUDIO SETTINGS
        masterSlider.value = PlayerPrefs.GetFloat("Master", 0.75f);
        musicSlider.value = PlayerPrefs.GetFloat("Music", 0.75f);
        sfxSlider.value = PlayerPrefs.GetFloat("SFX", 0.75f);

        SetMasterVolume(masterSlider.value);
        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);

        masterSlider.onValueChanged.AddListener(SetMasterVolume);
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    public void SetSeedFromInput(string seedInput)
    {
        string input = seedInputField.text;
        if (int.TryParse(input, out int customseed))
        {
            GlobalSettings.MapSeed = customseed;
            Debug.Log($"Custom Seed Set: {customseed}");
        }
        else
        {
            GlobalSettings.MapSeed = 0; // 0 means random seed
            Debug.Log("Invalid seed input. Using random seed.");
        }
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("resolutionIndex", resolutionIndex);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
        PlayerPrefs.SetInt("isFullscreen", isFullscreen ? 1 : 0);
    }

    public void SetMasterVolume(float volume)
    {
        float safeVolume = Mathf.Clamp(volume, 0.0001f, 1f);
        mainMixer.SetFloat("Master", Mathf.Log10(safeVolume) * 20);

        PlayerPrefs.SetFloat("Master", volume);
    }

    public void SetMusicVolume(float volume)
    {
        float safeVolume = Mathf.Clamp(volume, 0.0001f, 1f);
        mainMixer.SetFloat("Music", Mathf.Log10(safeVolume) * 20);
        PlayerPrefs.SetFloat("Music", volume);
    }

    public void SetSFXVolume(float volume)
    {
        float safeVolume = Mathf.Clamp(volume, 0.0001f, 1f);
        mainMixer.SetFloat("SFX", Mathf.Log10(safeVolume) * 20);
        PlayerPrefs.SetFloat("SFX", volume);
    }
}
