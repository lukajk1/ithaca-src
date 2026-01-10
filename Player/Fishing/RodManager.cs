using UnityEngine;
using UnityEngine.InputSystem;

public class RodManager : MonoBehaviour
{
    [HideInInspector] public bool rodEquipped;

    [SerializeField] private GameObject rod;
    [SerializeField] private GameObject bobber;
    [SerializeField] private GameObject fishingLine;

    [SerializeField] private InputActionReference equipRod;

    private void Awake()
    {
        UpdateEquip();
    }

    private void OnEnable()
    {
        equipRod.action.Enable();
        equipRod.action.performed += ToggleEquip;
    }

    private void OnDisable()
    {
        equipRod.action.Disable();
        equipRod.action.performed -= ToggleEquip;
    }

    public void SetLineAndBobber(bool value)
    {
        fishingLine.SetActive(value);
        bobber.SetActive(value);
    }

    void ToggleEquip(InputAction.CallbackContext ctx)
    {
        rodEquipped = !rodEquipped;

        UpdateEquip();
    }

    void UpdateEquip()
    {
        fishingLine.SetActive(rodEquipped);
        bobber.SetActive(rodEquipped);
        rod.SetActive(rodEquipped);
    }
}
