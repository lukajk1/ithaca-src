
using NaughtyAttributes;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewItem", menuName = "Ithaca/GenericItem")]
public class ItemData : ScriptableObject
{
    public string displayName = "unnamed";

    [ShowAssetPreview(70, 70)] 
    public Sprite icon;

    public Rarity rarity = Rarity.Common;
    public enum Type
    {
        Creature = 0,
        FishingRod = 5,
        Lure = 10,
        Charm = 25,
        Scraps = 50,
    }

    [SerializeField] protected Type _type;
    public virtual Type type
    {
        get => _type;
        set => _type = value;
    }

    [TextArea(2, 5)] public List<string> flavorTexts;

    [Tooltip("Value in microplastics")]
    public int value = 50;
}

public enum Rarity
{
    Common = 0,
    Uncommon = 5,
    Rare = 10,
    Legendary = 15,
    Mythical = 20,
}
