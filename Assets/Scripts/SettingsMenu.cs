using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField] private Material offStateMat;
    [SerializeField] private Material onStateMat;

    [SerializeField] private Slider slider;

    [SerializeField] private bool isVSyncDisabled = true;
    [SerializeField] private bool isFullscreen = true;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown dropdown;
    Resolution[] resolutions;

    private void Start()
    {
        resolutions = Screen.resolutions.Select(resolution => new Resolution { width = resolution.width, height = resolution.height }).Distinct().ToArray();

        dropdown.ClearOptions();

        List<string> options = new List<string>();

        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        dropdown.AddOptions(options);
        dropdown.value = currentResolutionIndex;
        dropdown.RefreshShownValue();
    }

    public void ChangeVSyncState(Image image)
    {
        if(isVSyncDisabled)
        {
            image.material = onStateMat;
            QualitySettings.vSyncCount = 1;
            isVSyncDisabled = false;
        }
        else
        {
            image.material = offStateMat;
            QualitySettings.vSyncCount = 0;
            isVSyncDisabled = true;
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("masterVolume", volume);
    }

    public void SetFullscreen(Image image)
    {
        if (isFullscreen)
        {
            image.material = onStateMat;
            Screen.fullScreen = true;
            isFullscreen = false;
        }
        else
        {
            image.material = offStateMat;
            Screen.fullScreen = false;
            isFullscreen = true;
        }
    }

    public void SetAntiAliasing(int index)
    {
        switch (index)
        {
            case 0:
                QualitySettings.antiAliasing = 0;
                break;
            case 1:
                QualitySettings.antiAliasing = 2;
                break;
            case 2:
                QualitySettings.antiAliasing = 4;
                break;
            case 3:
                QualitySettings.antiAliasing = 8;
                break;
        }
        print(QualitySettings.antiAliasing);
    }

    public void SaveState()
    {
        audioMixer.GetFloat("masterVolume", out float buffer);
        PlayerPrefs.SetFloat("masterVolume", buffer);
        print("SaveState");
    }
}
