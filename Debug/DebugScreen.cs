using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugScreen : MonoBehaviour
{
    [SerializeField] private Canvas canvas;
    [SerializeField] private InputActionReference toggleDebugScreenAction;

    [Header("Equips")]
    [SerializeField] private TextMeshProUGUI rodReadout;
    [SerializeField] private TextMeshProUGUI lureReadout;
    [SerializeField] private TextMeshProUGUI charmReadout;

    [Header("Stats")]
    [SerializeField] private TextMeshProUGUI biteSpeedReadout;

    public static DebugScreen i;
    private const string NULLSTRING = "-";

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
            // equips
            EquipmentSlots items = PlayerInfo.EquippedItems;
            rodReadout.text = items.FishingRod != null ? items.FishingRod.data.displayName : NULLSTRING;
            lureReadout.text = items.Lure != null ? items.Lure.data.displayName : NULLSTRING;
            charmReadout.text = items.Charm != null ? items.Charm.data.displayName : NULLSTRING;

            // stats
            biteSpeedReadout.text = $"{PlayerInfo.Stats.biteSpeedModifier}";
        }
    }

}
