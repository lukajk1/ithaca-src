using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class InteractPrompt : MonoBehaviour
{
    [SerializeField] private InputActionReference interactAction;

    [SerializeField] private GameObject interactPrompt;
    [SerializeField] public TextMeshProUGUI interactLabel;

    [SerializeField] private Vector2 relativeHidePos;
    private Vector2 ogPos;
    public static InteractPrompt i;

    private void Awake()
    {
        i = this;
        ogPos = interactPrompt.GetComponent<RectTransform>().anchoredPosition;

        LeanTween.alphaCanvas(interactPrompt.GetComponent<CanvasGroup>(), 0, 0);
        LeanTween.move(interactPrompt.GetComponent<RectTransform>(), ogPos + relativeHidePos, 0f);

        interactPrompt.SetActive(false);
    }

    public void Set(bool value, AInteractable.Type type = AInteractable.Type.Interact)
    {
        interactLabel.text = $"({interactAction.action.GetBindingDisplayString()}) {type.ToString()}";

        if (value) interactPrompt.SetActive(true);

        if (LeanTween.isTweening(interactPrompt))
            LeanTween.cancel(interactPrompt);

        LeanTween.alphaCanvas(interactPrompt.GetComponent<CanvasGroup>(), value ? 1f : 0f, 0.08f)
            .setOnComplete(() =>
            {
                if (!value) interactPrompt.SetActive(false);
            });
        LeanTween.move(interactPrompt.GetComponent<RectTransform>(), value ? ogPos : (ogPos + relativeHidePos), 0.15f);
    }
}
