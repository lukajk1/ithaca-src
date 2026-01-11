using UnityEngine;
using UnityEngine.EventSystems; // Required for IPointerClickHandler
using UnityEngine.InputSystem;
using UnityEngine.UI;

// 1. Implement IPointerClickHandler
public class ItemTile : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // The pointerOver flag is now technically redundant for clicks, 
    // but we'll keep it for the hover-driven logic (Tooltip, Sound, etc.).
    private bool pointerOver;
    public ConcreteItem item;

    [SerializeField] private Image image;
    [SerializeField] private Image glow;

    [HideInInspector] public bool sellable;
    [HideInInspector] public bool purchasable;
    [HideInInspector] public bool isInShop;

    public bool isEquipped;

    [Header("Audio")]
    [SerializeField] public AudioClip hoverClip;
    [SerializeField] public AudioClip trashClip;

    public void Init(ConcreteItem item)
    {
        this.item = item;
        image.sprite = item.data.icon;
        //glow.color = ColorLookup.GetColor(item.data.rarity);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Hover logic remains the same
        Tooltip.Show(item);
        SoundManager.Play(new SoundData(hoverClip, SoundData.Type.SFX));
        pointerOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Exit logic remains the same
        Tooltip.Hide();
        pointerOver = false;

    }
    void OnDestroy()
    {
        if (pointerOver) Tooltip.Hide();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            HandleLeftClick();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            HandleRightClick();
        }
    }

    private void HandleRightClick()
    {
        // === 1. SELL CONTEXT: Highest Priority (Must exit immediately) ===
        // If the item is in the player's inventory (not the shop's stock, hence !isInShop) 
        // AND a trade menu is open, right-click means SELL.
        if ((Game.context == Game.MenuContext.Shop || Game.context == Game.MenuContext.BuyBack) && !isInShop)
        {
            ShopManager.i.Sell(gameObject, this);
            return; // Sale action fulfilled, stop processing.
        }

        // === 2. INVENTORY CONTEXT (Equip or Unequip) ===
        // Proceed only if the game is in the Inventory context and the item is equipment.
        if (Game.context == Game.MenuContext.Inventory && item.data is EquipmentData)
        {
            // Check the current state of the item: is it equipped?
            if (isEquipped)
            {
                // UNEQUIP LOGIC
                // Only unequip if there is a free slot in the main inventory.
                if (InventoryManager.i.HasFreeSlot())
                {
                    // SetToFirstFreeSlot is the correct utility for moving the item out of the gear slot.
                    InventoryManager.i.SetToFirstFreeSlot(gameObject);
                    isEquipped = false;

                    PlayerInfo.EquippedItems.Unequip(item);
                    PlayerInfo.CalculateStats();
                }
                else
                {
                    // Optional: Show a message to the player that the inventory is full.
                    // Debug.LogWarning("Inventory full. Cannot unequip.");
                }
            }
            else // !isEquipped
            {
                // EQUIP LOGIC
                // The item is in the inventory and not equipped, so move it to the gear slot.
                // Equip method handles the swap (moving any old item out).
                InventoryManager.i.Equip(transform.parent.GetComponent<RectTransform>(), this, item);
                // The 'isEquipped = true' should ideally be set inside the InventoryManager.i.Equip method
                // after the move is confirmed, but is fine to be handled by the Equip method's caller too.
            }
        }

        if (Game.context == Game.MenuContext.Inventory && item.data.type == ItemData.Type.Creature)
        {
            SoundManager.Play(new SoundData(trashClip));
            Destroy(gameObject);
        }

        // No action for right-click on non-equipment items in inventory.
    }

    private void HandleLeftClick()
    {
        // buy context
        if (Game.context == Game.MenuContext.Shop && isInShop)
        {
            ShopManager.i.Buy(gameObject, this);
            isInShop = false;
        }
    }
}