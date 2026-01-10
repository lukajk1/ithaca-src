using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using TMPro;

public class OptionsMenu : Menu
{
    [SerializeField] private Canvas optionsMenu;

    [SerializeField] private SlideTransition slideTransition;

    [Header("Tabs")]
    [SerializeField] private AudioClip clickSFX;
    [SerializeField] private Button optionTab;
    [SerializeField] private Button keybindsTab;
    [SerializeField] private Color tabColorActive;
    [SerializeField] private Color tabColorInactive;

    [Header("UI Objects")]
    [SerializeField] private Button closeButton;
    [SerializeField] public TMP_Dropdown windowType;
    [SerializeField] public TMP_Dropdown resolution;

    [Header("Pages")]
    [SerializeField] private GameObject optionPageContent;
    [SerializeField] private GameObject keybindPageContent;

    public static OptionsMenu i;

    private void Awake()
    {
        i = this;

        closeButton.onClick.AddListener(() => Close());

        optionTab.onClick.AddListener(() => OpenTab(0));
        keybindsTab.onClick.AddListener(() => OpenTab(1));

        optionPageContent.SetActive(true);
        keybindPageContent.SetActive(false);

        windowType.onValueChanged.AddListener(OptionsManager.SetWindowType);
        resolution.onValueChanged.AddListener(OptionsManager.SetResolution);
    }
    private void Start()
    {
    }
    public static new bool Open()
    {

        Game.ModifyCursorUnlockList(true, i);
        //i.optionsMenu.gameObject.SetActive(true);
        i.slideTransition.Animate(true);

        LeanTween.delayedCall(0.05f, () => {
            OptionsManager.i.SetUIToLoadedValues();
        });
        return true;
    }

    public static new bool Close()
    {
        Game.ModifyCursorUnlockList(false, i);
        //i.optionsMenu.gameObject.SetActive(false);
        i.slideTransition.Animate(false);
        SoundManager.Play(new SoundData(i.clickSFX, SoundData.Type.SFX));

        OptionsManager.i.Save();

        return true;
    }

    public void OpenTab(int index)
    {
        bool optionSelected = index == 0;

        optionPageContent.SetActive(optionSelected);
        keybindPageContent.SetActive(!optionSelected);

        optionTab.GetComponent<Image>().color = optionSelected ? tabColorActive : tabColorInactive;
        keybindsTab.GetComponent<Image>().color = optionSelected ? tabColorInactive : tabColorActive;

        SoundManager.Play(new SoundData(clickSFX, SoundData.Type.SFX));
    }

}

/// <summary>
/// Class, defined in SettingsMenu.cs
/// </summary>

[System.Serializable]
public class GameSettings
{
    // 0 to 1
    public float MasterVolume;
    public float SoundFxVolume;
    public float MusicVolume;

    public bool VSync;
    public bool Pixellate;

    // int type to use indices, just a bit easier
    public int WindowType;
    public int Resolution;

    // constructor with default values
    public GameSettings()
    {
        MasterVolume = 0.5f;
        SoundFxVolume = 0.5f;
        MusicVolume = 0.5f;

        VSync = false;
        Pixellate = true;
        WindowType = 3;
        Resolution = 2;
    }
}
# region region
#endregion