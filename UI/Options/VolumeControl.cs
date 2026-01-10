using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField] public Slider slider;
    [SerializeField] private Button muteButton;
    [SerializeField] private TextMeshProUGUI valueLabel;

    [SerializeField] private Sprite muted;
    [SerializeField] private Sprite unmuted;

    [SerializeField] private AudioClip sliderTickChange;

    [Header("Slider Visual Tweak")]
    [SerializeField] private Color mutedColor;
    [SerializeField] private Image sliderFill;
    [SerializeField] private AudioClip muteButtonClick;

    private Color ogFillColor;

    private bool isMuted;

    // timing
    private float lastTickSoundTime;
    private const float tickSoundCooldown = 0.05f;

    [SerializeField] private VolType volType;
    public enum VolType
    {
        Master,
        Music,
        SFX,
        Ambient
    }

    private void Awake()
    {
        ogFillColor = sliderFill.color;
    }

    private void OnEnable()
    {
        slider.onValueChanged.AddListener(SliderValueChanged);
        muteButton.onClick.AddListener(ToggleMute);
    }

    private void OnDisable()
    {
        slider.onValueChanged.RemoveListener(SliderValueChanged);
        muteButton.onClick.RemoveListener(ToggleMute);
    }

    public void SliderValueChanged(float value)
    {
        valueLabel.text = Mathf.RoundToInt(value).ToString();

        if (Time.time - lastTickSoundTime >= tickSoundCooldown)
        {
            SoundManager.Play(new SoundData(sliderTickChange, SoundData.Type.SFX));
            lastTickSoundTime = Time.time;
        }

        if (!isMuted) SoundMixerManager.SetVolume(volType, value / 100f);
    }

    public void ToggleMute()
    {
        SetMuted(!isMuted);

    }
    public void SetMuted(bool value)
    {
        isMuted = value;
        muteButton.GetComponent<Image>().sprite = isMuted ? muted : unmuted;
        sliderFill.color = isMuted ? mutedColor : ogFillColor;

        if (isMuted)
        {
            SoundMixerManager.SetVolume(volType, 0);
        }
        else
        {
            SoundMixerManager.SetVolume(volType, slider.value / 100f);
        }

        SoundManager.Play(new SoundData(muteButtonClick, SoundData.Type.SFX));
    }

    public float GetSliderValue()
    {
        return slider.value;
    }

    public bool IsMuted()
    {
        return isMuted;
    }
}