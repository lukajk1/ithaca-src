using System;
using UnityEngine;

public class PanelSlide : MonoBehaviour
{
    [SerializeField] private RectTransform rectTransformToAnimate;
    [SerializeField] private CanvasGroup canvasGroupToFade;
    [SerializeField] private Vector2 relativeHidePosition;

    private Vector2 originalPosition;

    private void Awake()
    {
        originalPosition = rectTransformToAnimate.anchoredPosition;
        canvasGroupToFade.alpha = 0f;
    }
    public void Animate(bool value, Action onCompleteAction = null)
    {

        if (LeanTween.isTweening(rectTransformToAnimate))
            LeanTween.cancel(rectTransformToAnimate);

        if (value)
        {
            rectTransformToAnimate.gameObject.SetActive(true);
        }

        LeanTween.alphaCanvas(canvasGroupToFade, value ? 1f : 0f, 0.08f).setUseEstimatedTime(true);
        LeanTween.move(rectTransformToAnimate, value ? originalPosition : (originalPosition + relativeHidePosition), 0.15f)
            .setOnComplete(() =>
            {
                onCompleteAction?.Invoke();
            }).setUseEstimatedTime(true);
    }
}
