using TMPro;
using UnityEngine;

public class Announcements : MonoBehaviour
{
    public static Announcements i;

    [SerializeField] private CanvasGroup cg;
    [SerializeField] private float fadeLength = 0.2f;
    [SerializeField] public AudioClip jingle;

    [Header("Region")]
    [SerializeField] private TextMeshProUGUI regionTitle;
    [SerializeField] private float regionTitleLength = 1f;

    [Header("Notice")]
    [SerializeField] private TextMeshProUGUI smallerNotice;
    [SerializeField] private float noticeLength = 1f;

    private void Awake()
    {
        i = this;

        cg.alpha = 0f;
        regionTitle.gameObject.SetActive(false);
        smallerNotice.gameObject.SetActive(false);
    }

    /// <summary>
    /// Fades in a TextMeshPro object, displays it, and fades out the CanvasGroup.
    /// </summary>
    private void FadeInOut(TextMeshProUGUI textObject, string newText, float displayDuration)
    {
        // ignore if there is an ongoing tween
        if (LeanTween.isTweening(cg.gameObject)) return;

        cg.gameObject.SetActive(true);

        SoundManager.Play(new SoundData(Announcements.i.jingle));

        // 2. Hide all text objects, then set the new text and enable the target one
        regionTitle.gameObject.SetActive(false);
        smallerNotice.gameObject.SetActive(false);
        textObject.text = newText;
        textObject.gameObject.SetActive(true);

        // 3. Fade In
        LeanTween.alphaCanvas(cg, 1f, fadeLength)
            // 4. Delay/Display Duration
            .setOnComplete(() =>
            {
                // 5. Fade Out after the display duration
                LeanTween.alphaCanvas(cg, 0f, fadeLength)
                    .setDelay(displayDuration)
                    // 6. Disable the text object after fade-out is complete
                    .setOnComplete(() =>
                    {
                        textObject.gameObject.SetActive(false);
                    });
            });
    }

    // -------------------------------------------------------------------------
    // ## Public Methods
    // -------------------------------------------------------------------------

    /// <summary>
    /// Shows the region title with a fade-in, display, and fade-out effect.
    /// </summary>
    public void ShowRegionTitle(string title)
    {
        // Use the helper method with the region-specific objects and duration
        FadeInOut(regionTitle, title, regionTitleLength);
    }

    /// <summary>
    /// Shows the smaller notice with a fade-in, display, and fade-out effect.
    /// </summary>
    public void ShowNotice(string notice)
    {
        // Use the helper method with the notice-specific objects and duration
        FadeInOut(smallerNotice, notice, noticeLength);
    }
}

