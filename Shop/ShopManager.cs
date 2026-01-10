using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShopManager : MonoBehaviour
{
    [SerializeField] private GameObject itemTile;

    [Header("Bindings")]
    [SerializeField] private TextMeshProUGUI microplasticsText;
    [SerializeField] private ItemGridManager inventoryGrid;
    [SerializeField] private ItemGridManager shopGrid;

    [SerializeField] private AudioClip saleSFX;

    private List<ConcreteItem> buyBackItems = new();



    private float moneyAnimationTime = 1.2f;
    private int slotsInRow = 4;

    private int currentAnimatedValue; // Track the current animated money value
    private int animationId = -1; // Track the current animation

    public static ShopManager i;
    private void Awake()
    {
        i = this;
    }
    public void OnOpen(ShopData shopData)
    {
        inventoryGrid.CreateGrid(PlayerInfo.Inventory.Length);
        inventoryGrid.PopulateGrid(PlayerInfo.Inventory);

        microplasticsText.text = $"{PlayerInfo.Microplastics}";
        currentAnimatedValue = PlayerInfo.Microplastics; // to keep track of proper microplastics count 
        LoadShopProfile(shopData);
    }
    private void LoadShopProfile(ShopData shopData)
    {
        shopGrid.CreateGrid(RoundToMultiple(shopData.items.Count));
        shopGrid.PopulateShopGrid(shopData.items);
    }
    public void ExtractToInventory()
    {
        PlayerInfo.Inventory = inventoryGrid.ExtractItemsFromGrid();
        //foreach (var item in PlayerInfo.Inventory)
        //{
        //    Debug.Log(item);
        //}
    }
    int RoundToMultiple(int number)
    {
        int rounded = Mathf.CeilToInt((float)number / slotsInRow) * slotsInRow;
        int minSlots = slotsInRow * 2;
        
        return rounded > minSlots ? rounded : minSlots;
    }

    public void Buy(GameObject tile, ItemTile itemTile)
    {
        int price = SellItem.GetPrice(itemTile.item);

        if (price >= PlayerInfo.Microplastics)
        {
            // alert that the player doesn't have enough money
            return;
        }

        if (inventoryGrid.SetToFirstFreeSlot(tile))
        {
            SoundManager.Play(new SoundData(saleSFX, varyPitch: false));
            AnimateMoney(PlayerInfo.Microplastics, PlayerInfo.Microplastics - price);
        }
        else
        {
            // alert that there is no free space
        }
    }

    public void Sell(GameObject tile, ItemTile itemTile)
    {
        int price = SellItem.GetPrice(itemTile.item);
        SoundManager.Play(new SoundData(saleSFX, varyPitch: false));

        // Cancel existing animation if running
        if (animationId != -1)
        {
            LeanTween.cancel(animationId);
        }

        int newTotal = PlayerInfo.Microplastics + price;
        AnimateMoney(currentAnimatedValue, newTotal);
        buyBackItems.Add(itemTile.item);
        Destroy(tile);
    }

    private void AnimateMoney(int initial, int final)
    {
        animationId = LeanTween.value(gameObject, initial, final, moneyAnimationTime)
            .setEase(LeanTweenType.easeOutExpo)
            .setOnUpdate((float value) =>
            {
                currentAnimatedValue = (int)value;
                microplasticsText.text = $"{Mathf.RoundToInt(value)}";
            })
            .setOnComplete(() =>
            {
                animationId = -1;
                currentAnimatedValue = final;
            })
            .id;

        PlayerInfo.Microplastics = final;
    }




}
