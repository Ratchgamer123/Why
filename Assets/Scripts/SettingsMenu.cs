using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour, ISaveable
{
    [SerializeField] private Material offStateMat;
    [SerializeField] private Material onStateMat;

    [SerializeField] private UniversalRenderPipelineAsset pipelineAsset;

    [SerializeField] private bool isVSyncDisabled = true;
    [SerializeField] private bool isFullscreen = true;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private TMP_Dropdown dropdown;
    [SerializeField] private TMP_Dropdown dropdownMSAA;
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
        dropdownMSAA.value = pipelineAsset.msaaSampleCount;
        dropdownMSAA.RefreshShownValue();
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
                pipelineAsset.msaaSampleCount = 0;
                break;
            case 1:
                pipelineAsset.msaaSampleCount = 2;
                break;
            case 2:
                pipelineAsset.msaaSampleCount = 4;
                break;
            case 3:
                pipelineAsset.msaaSampleCount = 8;
                break;
        }
    }

    public object SaveState()
    {
        audioMixer.GetFloat("masterVolume", out float buffer);
        return new SaveData()
        {
            masterVolume = buffer
        };
    }

    public void LoadState(object state)
    {
        SaveData saveData = (SaveData)state;
        audioMixer.SetFloat("masterVolume", saveData.masterVolume);
    }

    private struct SaveData
    {
        public float masterVolume;
    }
}
