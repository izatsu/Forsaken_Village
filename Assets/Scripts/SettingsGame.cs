using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsGame : MonoBehaviour
{
    public TMP_Dropdown graphicsDrop, resoDrop;
    public Slider volumeSlider;

    private void Start()
    {
        InitializeSettings();
    }

    private void InitializeSettings()
    {
        if (PlayerPrefs.GetInt("settingsSaved", 0) == 0)
        {
            PlayerPrefs.SetInt("graphics", 2);
            PlayerPrefs.SetInt("resolution", 2);
            PlayerPrefs.SetFloat("masterVolume", 1f);
            SaveSettings();
        }

        // Graphics
        graphicsDrop.value = PlayerPrefs.GetInt("graphics");
        QualitySettings.SetQualityLevel(graphicsDrop.value);

        // Resolution
        resoDrop.value = PlayerPrefs.GetInt("resolution");
        SetResolution();

        // Volume
        volumeSlider.value = PlayerPrefs.GetFloat("masterVolume");
        SetVolume();
    }

    public void SetGraphics()
    {
        PlayerPrefs.SetInt("graphics", (int)graphicsDrop.value);
        QualitySettings.SetQualityLevel(graphicsDrop.value);
        SaveSettings();
    }

    public void SetResolution()
    {
        int resIndex = resoDrop.value;
        Vector2Int resolution = GetResolutionByIndex(resIndex);
        Screen.SetResolution(resolution.x, resolution.y, true);
        PlayerPrefs.SetInt("resolution", resIndex);
        SaveSettings();
    }

    private Vector2Int GetResolutionByIndex(int index)
    {
        switch (index)
        {
            case 0:
                return new Vector2Int(854, 480);
            case 1:
                return new Vector2Int(1280, 720);
            case 2:
                return new Vector2Int(1920, 1080);
            default:
                return new Vector2Int(1920, 1080);
        }
    }

    public void SetVolume()
    {
        float volume = volumeSlider.value;
        PlayerPrefs.SetFloat("masterVolume", volume);
        AudioListener.volume = volume;
        SaveSettings();
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetInt("settingsSaved", 1);
        PlayerPrefs.Save();
    }
}
