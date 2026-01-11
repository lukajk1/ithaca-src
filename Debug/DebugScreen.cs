using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugScreen : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private InputActionReference toggleDebugScreenAction;

    [SerializeField] private TextMeshProUGUI rodReadout;
    [SerializeField] private TextMeshProUGUI lureReadout;
    [SerializeField] private TextMeshProUGUI charmReadout;

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
    #endregion

    private void OnToggleDebugScreen(InputAction.CallbackContext context)
    {
        canvas.gameObject.SetActive(!canvas.gameObject.activeSelf);
    }

    private void Update()
    {
        if (canvas.gameObject.activeSelf)
        {
            EquipmentSlots items = PlayerInfo.EquippedItems;

            rodReadout.text = items.FishingRod != null ? items.FishingRod.data.displayName : "None";
            lureReadout.text = items.Lure != null ? items.Lure.data.displayName : "None";
            charmReadout.text = items.Charm != null ? items.Charm.data.displayName : "None";
        }
    }

}
