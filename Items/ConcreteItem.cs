using UnityEngine;
using System.Collections.Generic;

public class ConcreteItem
{
    private ItemData _data;
    public ItemData data
    {
        get => _data;
        set
        {
            _data = value;
            selectedFlavorText = (_data?.flavorTexts?.Count > 0)
                ? _data.flavorTexts[Random.Range(0, _data.flavorTexts.Count)]
                : null;
        }
    }
    public string selectedFlavorText { get; private set; }

    public ConcreteItem(ItemData data)
    {
        this.data = data;
    }

    // fish constructor

    public float quality;
    public ConcreteItem(FishData fishData, float quality)
    {
        this.data = fishData;
        this.quality = quality;
    }
}