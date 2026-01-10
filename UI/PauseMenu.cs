using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{

    [SerializeField] private GameObject pauseCanvas;

    [SerializeField] private GameObject backdrop;

    private bool isOpen; 
    
    [SerializeField] private GameObject menu;
    [SerializeField] private Vector2 relativeHidePos;
    private Vector2 ogPos;

    [SerializeField] private Button backToGame;
    [SerializeField] private Button options;
    [SerializeField] private Button quit;

    [SerializeField] private AudioClip onPauseClip;
    [SerializeField] private AudioClip onResumeClip;

    private void Awake()
    {
        pauseCanvas.SetActive(false);

        backToGame.onClick.AddListener(() => SetEscMenu(false));
        options.onClick.AddListener(() => OptionsMenu.Open());
        quit.onClick.AddListener(() => Quit());

        LeanTween.alphaCanvas(pauseCanvas.GetComponent<CanvasGroup>(), 0, 0);

        ogPos = menu.GetComponent<RectTransform>().anchoredPosition;

        LeanTween.alphaCanvas(menu.GetComponent<CanvasGroup>(), 0, 0);
        LeanTween.move(menu.GetComponent<RectTransform>(), ogPos + relativeHidePos, 0f);
    }

    private void Update()
    {
        // no reason to tie this to actions system, best if esc isn't rebindable.
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            SetEscMenu(!isOpen);
            ToggleHUD.i.SetEnabled(true);
        }
    }

    private void Quit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }
    private void SetEscMenu(bool value)
    {
        if (value) pauseCanvas.SetActive(value);
        isOpen = value;

        Game.ModifyPauseList(value, this);
        Game.ModifyCursorUnlockList(value, this);

        SoundMixerManager.SetEnvVolume(value ? 0 : 1);
        SoundManager.Play(new SoundData(value ? onPauseClip : onResumeClip, SoundData.Type.SFX));


        if (LeanTween.isTweening(menu))
            LeanTween.cancel(menu);

        LeanTween.alphaCanvas(pauseCanvas.GetComponent<CanvasGroup>(), value ? 1f : 0f, 0.08f).setUseEstimatedTime(true);

        LeanTween.alphaCanvas(menu.GetComponent<CanvasGroup>(), value ? 1f : 0f, 0.08f).setUseEstimatedTime(true);
        LeanTween.move(menu.GetComponent<RectTransform>(), value ? ogPos : (ogPos + relativeHidePos), 0.15f)
            .setOnComplete(() =>
        {
            pauseCanvas.SetActive(value);
        }).setUseEstimatedTime(true); 

    }
}
