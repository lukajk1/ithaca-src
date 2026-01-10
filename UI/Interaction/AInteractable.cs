using UnityEngine;

public abstract class AInteractable : MonoBehaviour
{
    public abstract void Interact();

    public enum Type
    {
        Interact = 0,
        Talk = 10,
        Sit = 20,

    }

    public virtual Type interactType => Type.Interact;

    private bool _isDisplaying;
    public bool isDisplaying
    {
        get => _isDisplaying; 
        set
        {
            if (_isDisplaying != value)
            {
                _isDisplaying = value;
                InteractPrompt.i.Set(value, interactType);
            }
        }
    }
}
