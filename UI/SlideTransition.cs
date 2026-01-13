using UnityEngine;

public class SlideTransition : MonoBehaviour
{
    [SerializeField] protected Canvas canvas;
    [SerializeField] protected GameObject objectToAnimate;
    [SerializeField] protected Vector2 relativeHidePos;
    [SerializeField] protected float slideSpeed = 0.08f;
    protected Vector2 ogPos;

    public virtual void Awake()
    {
        canvas.gameObject.SetActive(false);

        ogPos = objectToAnimate.GetComponent<RectTransform>().anchoredPosition;

        LeanTween.alphaCanvas(objectToAnimate.GetComponent<CanvasGroup>(), 0, 0).setUseEstimatedTime(true);
        LeanTween.move(objectToAnimate.GetComponent<RectTransform>(), ogPos + relativeHidePos, 0f).setUseEstimatedTime(true);

    }

    public void Animate(bool value)
    {
        if (value) canvas.gameObject.SetActive(value);

        if (LeanTween.isTweening(objectToAnimate))
            LeanTween.cancel(objectToAnimate);

        LeanTween.alphaCanvas(objectToAnimate.GetComponent<CanvasGroup>(), value ? 1f : 0f, 0.08f).setUseEstimatedTime(true);
        LeanTween.move(objectToAnimate.GetComponent<RectTransform>(), value ? ogPos : (ogPos + relativeHidePos), slideSpeed)
            .setOnComplete(() =>
            {
                canvas.gameObject.SetActive(value);
            }).setUseEstimatedTime(true);
    }
}
