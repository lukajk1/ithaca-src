using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryMenu : SlideTransition
{

    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;

    [SerializeField] private InputActionReference openInventory;
    public override void Awake()
    {
        base.Awake();
    }
    private void OnEnable()
    {
        openInventory.action.Enable();
        openInventory.action.performed += OnOpenInventoryPressed;
    }

    private void OnDisable()
    {
        openInventory.action.Disable();
        openInventory.action.performed -= OnOpenInventoryPressed;
    }

    void OnOpenInventoryPressed(InputAction.CallbackContext ctx)
    {

        bool isOpening = !canvas.gameObject.activeSelf;

        // prevent inventory from opening if there is already existing menus
        if (Game.context != Game.MenuContext.None && isOpening) { return; }

        Animate(isOpening);
        SoundManager.Play(new SoundData(isOpening ? openClip : closeClip, SoundData.Type.SFX));

        if (isOpening) InventoryManager.i.Open();
        else InventoryManager.i.Close();

        Game.ModifyCursorUnlockList(isOpening, this);
        Game.context = isOpening ? Game.MenuContext.Inventory : Game.MenuContext.None;
    }
}
