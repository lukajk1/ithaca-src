using UnityEngine;

public class SlidingAnimationInteractable : AInteractable
{
    [SerializeField] private Vector3 targetPosition;
    [SerializeField] private float tweenDuration = 1f;
    [SerializeField] private AudioClip slideDoor;
    private Vector3 originalPos;
    private bool open;
    private void Awake()
    {
        originalPos = transform.localPosition;
    }
    public override void Interact()
    {
        open = !open;
        if (open)
        {
            LeanTween.moveLocal(gameObject, targetPosition, tweenDuration).setEase(LeanTweenType.easeOutQuart);
        }
        else
        {
            LeanTween.moveLocal(gameObject, originalPos, tweenDuration).setEase(LeanTweenType.easeOutQuart);
        }
        SoundManager.Play(new SoundData(slideDoor, SoundData.Type.SFX));
    }

}
