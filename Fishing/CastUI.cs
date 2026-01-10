using UnityEngine;
using UnityEngine.UI;

public class CastUI : MonoBehaviour
{
    [SerializeField] private Canvas castCanvas;
    [SerializeField] private Slider slider;
    [SerializeField] private PanelSlide panelSlide;
    [SerializeField] private Image sliderBody;
    [SerializeField] private Gradient gradient;
    public float sliderValue => slider.value;
    private int castTweenId;

    // singleton
    public static CastUI i;
    private void Awake()
    {
        i = this;

        castCanvas.gameObject.SetActive(false);

    }

    public void StartCast(float speedMultiplier)
    {

        if (castTweenId != 0)
        {
            LeanTween.cancel(castTweenId);
        }

        castCanvas.gameObject.SetActive(true);
        panelSlide.Animate(true);

        // Calculates duration: 1.0f is the base time. Dividing by the multiplier
        // speeds it up or slows it down.
        float duration = 1.0f / speedMultiplier;

        // Safety check to prevent division by zero or negative time
        if (duration <= 0) duration = 0.01f;

        castTweenId = LeanTween.value(gameObject, 0f, 1f, duration)
            .setOnUpdate((float value) => {
                slider.value = value;
                sliderBody.color = gradient.Evaluate(value);
            })
            .setEase(LeanTweenType.easeInOutCubic)
            .setLoopPingPong(-1)
            .id;
    }

    public void StopCast()
    {
        LeanTween.cancel(castTweenId);
        castTweenId = 0;

        panelSlide.Animate(false, () => castCanvas.gameObject.SetActive(false));
    }
}
