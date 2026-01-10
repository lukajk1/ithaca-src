using UnityEngine;

public class ScreenFade : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    public static ScreenFade i;

    private void Awake()
    {
        i = this;

        canvasGroup.alpha = 0f;
        canvasGroup.gameObject.SetActive(false);
    }

    /// <summary>
    /// Fades the screen to black, and then fades back to clear.
    /// </summary>
    /// <param name="duration">The total time for the fade-in and fade-out cycle.</param>
    /// <param name="easeInType">The easing style for the fade-in (0 to 1 alpha).</param>
    /// <param name="easeOutType">The easing style for the fade-out (1 to 0 alpha).</param>
    public void Fade(float duration, LeanTweenType easeInType, LeanTweenType easeOutType, System.Action midwayAction = null, System.Action endAction = null)
    {
        // Cancel the tween if it's already running to prevent overlap
        if (LeanTween.isTweening(canvasGroup.gameObject)) return;

        // Ensure the CanvasGroup is visible before starting the fade
        canvasGroup.gameObject.SetActive(true);

        // Calculate the time for the fade-in and fade-out segments
        float segmentDuration = duration / 2f;

        // 1. Fade In (alpha 0 to 1)
        LeanTween.alphaCanvas(canvasGroup, 1f, segmentDuration)
            .setEase(easeInType)
            // 2. Once the fade-in is complete, start the fade-out
            .setOnComplete(() =>
            {
                midwayAction?.Invoke();
                LeanTween.alphaCanvas(canvasGroup, 0f, segmentDuration)
                    .setEase(easeOutType)
                    .setOnComplete(() =>
                    {
                        // Set the object inactive now that the screen is clear
                        canvasGroup.gameObject.SetActive(false);

                        // 3. EXECUTE THE OPTIONAL CALLBACK HERE
                        endAction?.Invoke();
                    });
            });
    }
}
