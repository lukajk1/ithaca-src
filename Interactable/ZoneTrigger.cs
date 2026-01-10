using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{
    [SerializeField] AInteractable interactable;
    private void OnTriggerEnter(Collider other)
    {
        interactable?.Interact();
    }
    private void OnTriggerExit(Collider other)
    {
        interactable?.Interact();
    }
}
