using UnityEngine;
using UnityEngine.InputSystem;

public class CheckForInteractable : MonoBehaviour
{
    //[SerializeField] private Camera fpCamera;
    //[SerializeField] private InteractPrompt interactPrompt;
    //[SerializeField] private InputActionReference interactAction;

    //private const float Range = 3.6f;
    //private RaycastHit hit;

    //private AInteractable currentInteractable;

    //private void Start()
    //{
    //    //interactPrompt.interactLabel.text = $"({interactAction.action.GetBindingDisplayString()}) Interact";
    //}
    //private void OnEnable()
    //{
    //    interactAction.action.Enable();
    //    interactAction.action.performed += OnInteract;
    //}

    //private void OnDisable()
    //{
    //    interactAction.action.performed -= OnInteract;
    //    interactAction.action.Disable();
    //}

    //private void Update()
    //{
    //    ScanForInteractable();
    //}

    //private void ScanForInteractable()
    //{
    //    AInteractable detected = null;

    //    if (Physics.Raycast(fpCamera.transform.position, fpCamera.transform.forward, out hit, Range))
    //    {
    //        detected = hit.collider.GetComponent<AInteractable>();
    //    }

    //    bool hasChanged = currentInteractable != detected;
    //    currentInteractable = detected;

    //    if (hasChanged)
    //    {
    //        //interactPrompt.Set(currentInteractable != null);
    //        interactPrompt.interactLabel.text = $"({interactAction.action.GetBindingDisplayString()}) Interact";
    //    }
    //}

    //private void OnInteract(InputAction.CallbackContext context)
    //{
    //    //Debug.Log("interact key pressed");

    //    if (currentInteractable != null)
    //    {
    //        currentInteractable.OnInteractPressed();
    //    }
    //}
}
