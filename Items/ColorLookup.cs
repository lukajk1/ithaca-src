using Unity.Burst.Intrinsics;
using UnityEngine;

public class ColorLookup : MonoBehaviour
{
    [SerializeField] private Color common;
    [SerializeField] private Color uncommon;
    [SerializeField] private Color rare;
    [SerializeField] private Color legendary;
    [SerializeField] private Color mythical;

    public static ColorLookup i;

    private void Awake()
    {
        i = this;
    }

    public static Color LookupRarity(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common:
                return i.common;
            case Rarity.Uncommon:
                return i.uncommon;
            case Rarity.Rare:
                return i.rare;
            case Rarity.Legendary:
                return i.legendary;
            case Rarity.Mythical:
                return i.mythical;
            default:
                return Color.white;
        }
    }
}