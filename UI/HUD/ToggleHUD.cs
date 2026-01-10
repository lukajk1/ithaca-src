using UnityEngine;
using UnityEngine.InputSystem;

public class ToggleHUD : MonoBehaviour
{
    [SerializeField] private Canvas HUDCanvas;
    [SerializeField] private InputActionReference toggleHUD;
    private bool hudEnabled = true; // visible by default. No need to store it since probably better that it restores on game/scene refresh lol
    public static ToggleHUD i;
    private void Awake()
    {
        i = this;
    }
    private void OnEnable()
    {
        toggleHUD.action.Enable();
        toggleHUD.action.performed += OnTogglePressed;
    }

    private void OnDisable()
    {
        toggleHUD.action.Disable();
        toggleHUD.action.performed -= OnTogglePressed;
    }

    public void OnTogglePressed(InputAction.CallbackContext context)
    {
        SetEnabled(!hudEnabled);
    }

    public void SetEnabled(bool enabled)
    {
        HUDCanvas.gameObject.SetActive(enabled);
        hudEnabled = HUDCanvas.gameObject.activeSelf;
    }
}
