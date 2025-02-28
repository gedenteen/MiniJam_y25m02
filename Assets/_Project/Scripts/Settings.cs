using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class Settings : UiPanel
{
    public static Settings instance {get; private set;} = null;

    [Header("References to objects")]
    [SerializeField] private Toggle toggleFullScreen;
    [SerializeField] private TextMeshProUGUI textResolutions;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Slider sliderMusicVolume;
    [SerializeField] private Slider sliderSoundsVolume;

    [Header("Refrences to assets")]
    [SerializeField] private AudioMixer audioMixer;

    // Private fields
    private RefreshRate currentRefreshRate;
    private int currentResolutionIndex;
    private List<Resolution> filteredResolutions;

    private bool isInitialized = false;

    // Strings for PlayerPrefs
    private readonly string pp_musicVolume = "MusicVolume";
    private readonly string pp_soundVolume = "SoundVolume";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }

#if !UNITY_EDITOR && (UNITY_WEBGL || UNITY_IOS || UNITY_ANDROID)
        toggleFullScreen.gameObject.SetActive(false);
        textResolutions.gameObject.SetActive(false);
        resolutionDropdown.gameObject.SetActive(false);
#endif
    }

    private void Start()
    {
        StartSetAudio();
        StartSetResolutions();

        isInitialized = true;
    }

    public void StartSetAudio()
    {
        // Default values of settings
        float musicVolume = 1f;
        float soundVolume = 1f;

        // Get player values, if he changed them
        if (PlayerPrefs.HasKey(pp_musicVolume))
        {
            musicVolume = PlayerPrefs.GetFloat(pp_musicVolume);
        }
        if (PlayerPrefs.HasKey(pp_soundVolume))
        {
            soundVolume = PlayerPrefs.GetFloat(pp_soundVolume);
        }

        Debug.Log($"Settings: Start: musicVolume={musicVolume} soundVolume={soundVolume}");
        SetMusicVolume(musicVolume);
        sliderMusicVolume.value = musicVolume;
        SetSoundsVolume(soundVolume);
        sliderSoundsVolume.value = soundVolume;
    }

    private void StartSetResolutions()
    {
        // Get resolutuons and refresh rate (Hz)
        Resolution[] resolutions = Screen.resolutions;
        currentRefreshRate = Screen.currentResolution.refreshRateRatio;
        filteredResolutions = new List<Resolution>();

        // Cycle for filtering resolutions
        for (int i = 0; i < resolutions.Length; i++)
        {
            //Debug.Log($"{resolutions[i].width}x{resolutions[i].height} {resolutions[i].refreshRateRatio.value} Hz");
            if (i + 1 < resolutions.Length)
            {
                if (resolutions[i].width != resolutions[i + 1].width || resolutions[i].height != resolutions[i + 1].height)
                {
                    filteredResolutions.Add(resolutions[i]);
                }
            }
            else
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        // Cycle for add filtered resolutuions to dropdown
        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            int hz = Mathf.RoundToInt((float)filteredResolutions[i].refreshRateRatio.value);
            string optionText = string.Concat(filteredResolutions[i].width, "x", filteredResolutions[i].height, " ", hz.ToString(), "Hz");
            options.Add(optionText);

            if (filteredResolutions[i].width == Screen.currentResolution.width && filteredResolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    #region MethodsForButtons
    public void SetResolution(int resolutionIndex)
    {
        Resolution res = filteredResolutions[resolutionIndex];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);

        if (isInitialized)
        {
            PlayerPrefs.SetFloat(pp_musicVolume, volume);
        }
    }

    public void SetSoundsVolume(float volume)
    {
        audioMixer.SetFloat("SoundsVolume", Mathf.Log10(volume) * 20);

        if (isInitialized)
        {
            PlayerPrefs.SetFloat(pp_soundVolume, volume);
        }
    }

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void ShowFps(bool isShowFps)
    {
        FpsCounter.eventActivate.Invoke(isShowFps);
    }

    public void CloseSettings()
    {
        Activate(false);
    }
    #endregion
}
