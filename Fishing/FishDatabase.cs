using UnityEngine;
using System.Collections.Generic;

public class FishDatabase : MonoBehaviour
{
    [SerializeField] private List<FishData> freshwaterFish;
    [SerializeField] private List<FishData> saltwaterFish;
    [SerializeField] private List<FishData> deepFish;
    [SerializeField] private List<FishData> wetlandsFish;

    public static FishDatabase i;

    private void Awake()
    {
        i = this;
    }

    public FishData GetFish(DayPeriod dayPeriod, FishData.Zone zone)
    {
        List<FishData> zoneList = GetZoneList(zone);
        return RandomFishByRarity(zoneList);
    }

    public FishData GetFishByZoneAndRarity(DayPeriod dayPeriod, FishData.Zone zone, Rarity rarity)
    {
        List<FishData> zoneList = GetZoneList(zone);

        // Filter fish by rarity
        List<FishData> rarityPool = new List<FishData>();
        foreach (var fish in zoneList)
        {
            if (fish.rarity == rarity)
            {
                rarityPool.Add(fish);
            }
        }

        // If no fish at this rarity, return null to trigger reroll
        if (rarityPool.Count == 0)
        {
            return null;
        }

        // Random fish from the rarity pool
        return rarityPool[Random.Range(0, rarityPool.Count)];
    }

    private List<FishData> GetZoneList(FishData.Zone zone)
    {
        switch (zone)
        {
            case FishData.Zone.Freshwater:
                return freshwaterFish;
            case FishData.Zone.Saltwater:
                return saltwaterFish;
            case FishData.Zone.Deep:
                return deepFish;
            case FishData.Zone.Wetlands:
                return wetlandsFish;
            default:
                return freshwaterFish; // shouldn't ever reach here
        }
    }

    private FishData RandomFishByRarity(List<FishData> availableFish)
    {
        float totalWeight = 0;
        foreach (var fish in availableFish)
        {
            totalWeight += fish.weightedRarity;
        }

        float random = Random.Range(0, totalWeight);
        float currentWeight = 0;

        foreach (var fish in availableFish)
        {
            currentWeight += fish.weightedRarity;
            if (random < currentWeight)
                return fish;
        }

        // Fallback (shouldn't reach here)
        return availableFish[availableFish.Count - 1];
    }
}
