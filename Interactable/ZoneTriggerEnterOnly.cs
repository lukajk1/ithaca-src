using UnityEngine;

public class ZoneTriggerEnterOnly : MonoBehaviour
{
    [SerializeField] AInteractable interactable;
    private void OnTriggerEnter(Collider other)
    {
        interactable?.Interact();
    }
}