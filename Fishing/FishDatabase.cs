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
        List<FishData> zoneList;

        switch (zone)
        {
            case FishData.Zone.Freshwater:
                zoneList = freshwaterFish;
                break;
            case FishData.Zone.Saltwater:
                zoneList = saltwaterFish;
                break;
            case FishData.Zone.Deep:
                zoneList = deepFish;
                break;
            case FishData.Zone.Wetlands:
                zoneList = wetlandsFish;
                break;
            default:
                zoneList = freshwaterFish; // shouldn't ever reach here
                break;
        }

        return RandomFishByRarity(zoneList);
    }

    // I suppose fish should just have a standard rarity assigned by their rarity tier.. except for mythical fish?
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
