using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    public static OptionsManager i;
    private OptionsData optionsData;

    [SerializeField] private VolumeControl masterControl;
    [SerializeField] private VolumeControl sfxControl;
    [SerializeField] private VolumeControl envControl;
    [SerializeField] private VolumeControl musicControl;

    [SerializeField] private Slider mouseSens;

    private void Awake()
    {
        i = this;
        optionsData = OptionsData.CreateDefault();
    }

    private void Start()
    {
        Load(); // called in start so that soundmixermanager is guaranteed to already be initialized when populating values
    }

    public void SetUIToLoadedValues()
    {
        masterControl.slider.value = optionsData.masterVol;
        sfxControl.slider.value = optionsData.sfxVol;
        envControl.slider.value = optionsData.envVol;
        musicControl.slider.value = optionsData.musicVol;

        masterControl.SetMuted(optionsData.masterMuted);
        sfxControl.SetMuted(optionsData.sfxMuted);
        envControl.SetMuted(optionsData.envMuted);
        musicControl.SetMuted(optionsData.musicMuted);

        mouseSens.value = optionsData.mouseSens;

        OptionsMenu.i.resolution.value = optionsData.res;
        OptionsMenu.i.windowType.value = optionsData.windowType;

    }

    private void OptionsDataFromUIValues()
    {
        optionsData.masterVol = masterControl.GetSliderValue();
        optionsData.sfxVol = sfxControl.GetSliderValue();
        optionsData.envVol = envControl.GetSliderValue();
        optionsData.musicVol = musicControl.GetSliderValue();

        optionsData.masterMuted = masterControl.IsMuted();
        optionsData.sfxMuted = sfxControl.IsMuted();
        optionsData.envMuted = envControl.IsMuted();
        optionsData.musicMuted = musicControl.IsMuted();

        optionsData.mouseSens = mouseSens.value;

        optionsData.res = OptionsMenu.i.resolution.value;
        optionsData.windowType = OptionsMenu.i.windowType.value;
    }

    private void SetGameValues()
    {
        SoundMixerManager.SetMasterVolume(optionsData.masterVol / 100f);
        SoundMixerManager.SetSFXVolume(optionsData.sfxVol / 100f);
        SoundMixerManager.SetEnvVolume(optionsData.envVol / 100f);
        SoundMixerManager.SetMusicVolume(optionsData.musicVol / 100f);

        SetResolution(optionsData.res);
        SetWindowType(optionsData.windowType);
    }

    public static void SetResolution(int index)
    {
        (int width, int height) res;

        switch (index)
        {
            case 0:
                res = (2560, 1440);
                break;
            case 1:
                res = (1920, 1080);
                break;
            case 2:
                res = (1600, 900);
                break;
            case 3:
                res = (1280, 720);
                break;
            default:
                res = (1600, 900);
                break;
        }

        Screen.SetResolution(res.width, res.height, Screen.fullScreenMode);
    }

    public static void SetWindowType(int index)
    {
        FullScreenMode mode;

        switch (index)
        {
            case 0:
                mode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                mode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                mode = FullScreenMode.MaximizedWindow;
                break;
            case 3:
                mode = FullScreenMode.Windowed;
                break;
            default:
                mode = FullScreenMode.Windowed;
                break;
        }

        Screen.fullScreenMode = mode;
    }

    #region save load
    private static string SaveFileName()
    {
        return Path.Combine(Application.persistentDataPath, "game-settings.save");
    }

    public void Save()
    {
        i.OptionsDataFromUIValues(); 
        File.WriteAllText(SaveFileName(), JsonUtility.ToJson(i.optionsData, true));
    }

    private static void Load()
    {
        string saveFilePath = SaveFileName();

        if (File.Exists(saveFilePath))
        {
            try
            {
                string saveContent = File.ReadAllText(saveFilePath);
                i.optionsData = JsonUtility.FromJson<OptionsData>(saveContent);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning($"Failed to load settings: {e.Message}. Using defaults.");
                i.optionsData = OptionsData.CreateDefault(); // Use defaults if load fails
            }
        }
        else
        {
            Debug.Log("No save file found. Using default settings.");
            i.optionsData = OptionsData.CreateDefault(); // Use defaults if no save file
        }

        i.SetGameValues();
    }
    #endregion

    public void ResetToDefaults()
    {
        optionsData = OptionsData.CreateDefault();
        SetUIToLoadedValues();
    }
}

[System.Serializable]
public struct OptionsData
{
    public bool masterMuted;
    public bool sfxMuted;
    public bool envMuted;
    public bool musicMuted;

    public float masterVol;
    public float sfxVol;
    public float envVol;
    public float musicVol;
    public float mouseSens;

    public int res;
    public int windowType;

    public static OptionsData CreateDefault()
    {
        return new OptionsData
        {
            masterMuted = false,
            sfxMuted = false,
            envMuted = false,
            musicMuted = false,

            masterVol = 55f,
            sfxVol = 55f,
            envVol = 55f,
            musicVol = 55f,

            mouseSens = 55f,

            res = 2,
            windowType = 3
        };
    }
}