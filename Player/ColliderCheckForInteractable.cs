using UnityEngine;
using UnityEngine.InputSystem;

public class ColliderCheckForInteractable : MonoBehaviour
{
    [SerializeField] private InputActionReference interactAction;

    private InteractTrigger currentTrigger;
    private void OnEnable()
    {
        interactAction.action.Enable();
        interactAction.action.performed += OnInteract;
    }

    private void OnDisable()
    {
        interactAction.action.performed -= OnInteract;
        interactAction.action.Disable();
    }

    private void OnTriggerEnter(Collider col)
    {
        if (col.TryGetComponent<InteractTrigger>(out InteractTrigger trigger))
        {
            currentTrigger = trigger;
            trigger.SetVisible(true);
        }
    }

    private void OnTriggerExit(Collider col)
    {
        if (currentTrigger != null) currentTrigger.SetVisible(false); 

        currentTrigger = null;
    }

    private void OnTriggerStay(Collider col)
    {
        //if (NPCMenu.i.isOpen || PixelCrushers.DialogueSystem.DialogueManager.isConversationActive) // this is definitely not good practice.. referencing this same dependency in multiple places..
        //{
        //    currentTrigger.isDisplaying = false;
        //}

    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        if (currentTrigger != null)
        {
            currentTrigger.Interact();
        }
    }
}
