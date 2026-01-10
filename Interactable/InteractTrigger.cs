using NaughtyAttributes;
using UnityEngine;

public class InteractTrigger : MonoBehaviour
{
    [SerializeField] private AInteractable interactable;

    [SerializeField] private bool usesHighlight;

    [ShowIf("usesHighlight")]
    [SerializeField] private HighlightController highlight;

    //public bool triggerDisabled;

    public void SetVisible(bool value)
    {
        if (usesHighlight)
        {
            highlight.SetHighlight(value);
        }

        interactable.isDisplaying = value;
    }
    public void Interact()
    {
        interactable.Interact();

        highlight.SetHighlight(false);
        interactable.isDisplaying = false;
    }
}
