using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopMenu : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip openClip;
    [SerializeField] private AudioClip closeClip;

    // refs
    [SerializeField] private Canvas shopCanvas;
    [SerializeField] private Button leaveShopButton;

    [SerializeField] private Canvas tooltipCanvas;
    [SerializeField] private Camera main;
    [SerializeField] private Camera convoCam;



    public static ShopMenu i;
    private void Awake()
    {
        i = this;

        shopCanvas.gameObject.SetActive(false);
        leaveShopButton.onClick.AddListener(OnLeaveClick);
    }

    void OnDestroy()
    {
        leaveShopButton.onClick.RemoveListener(OnLeaveClick);
    }

    private void OnLeaveClick()
    {
        SetMenu(false, null);
        NPCMenu.i.SetVisible(true);
    }
    public void SetMenu(bool value, ShopData shopData)
    {

        if (value)
        {
            ShopManager.i.OnOpen(shopData);
        }
        else
        {
            ShopManager.i.ExtractToInventory();
        }

        tooltipCanvas.worldCamera = value ? convoCam : main;

        Game.ModifyCursorUnlockList(value, this);
        Game.context = value ? Game.MenuContext.Shop : Game.MenuContext.None;
        shopCanvas.gameObject.SetActive(value);
    }
}
