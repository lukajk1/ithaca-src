using UnityEngine;
using UnityEngine.InputSystem;

public class DebugScreen : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private InputActionReference toggleDebugScreenAction;

    public static DebugScreen i;
    #region setup

    private void Awake()
    {
        i = this;
    }

    private void OnEnable()
    {
        if (toggleDebugScreenAction != null)
        {
            toggleDebugScreenAction.action.Enable();
            toggleDebugScreenAction.action.performed += OnToggleDebugScreen;
        }
    }

    private void OnDisable()
    {
        if (toggleDebugScreenAction != null)
        {
            toggleDebugScreenAction.action.performed -= OnToggleDebugScreen;
            toggleDebugScreenAction.action.Disable();
        }
    }

    void Start()
    {
        canvas.gameObject.SetActive(false);
    }

    private void OnToggleDebugScreen(InputAction.CallbackContext context)
    {
        canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);
    }
    #endregion



}
