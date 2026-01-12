using UnityEngine;
using System.Collections.Generic;

public class FishDatabase : MonoBehaviour
{
    [SerializeField] private bool printDebugStatements;

    [SerializeField] private List<FishData> freshwaterFish;
    [SerializeField] private List<FishData> saltwaterFish;
    [SerializeField] private List<FishData> deepFish;
    [SerializeField] private List<FishData> wetlandsFish;

    // Precomputed rarity pools: [zone][rarity]
    private Dictionary<FishData.Zone, Dictionary<Rarity, List<FishData>>> rarityPools;

    public static FishDatabase i;

    private void Awake()
    {
        i = this;
    }

    private void Start()
    {
        PrecomputeRarityPools();
    }

    private void PrecomputeRarityPools()
    {
        rarityPools = new Dictionary<FishData.Zone, Dictionary<Rarity, List<FishData>>>();

        // Precompute for each zone
        PrecomputeZoneRarityPools(FishData.Zone.Freshwater, freshwaterFish);
        PrecomputeZoneRarityPools(FishData.Zone.Saltwater, saltwaterFish);
        PrecomputeZoneRarityPools(FishData.Zone.Deep, deepFish);
        PrecomputeZoneRarityPools(FishData.Zone.Wetlands, wetlandsFish);
    }

    private void PrecomputeZoneRarityPools(FishData.Zone zone, List<FishData> fishList)
    {
        Dictionary<Rarity, List<FishData>> rarityDict = new Dictionary<Rarity, List<FishData>>();

        // Initialize lists for each rarity
        rarityDict[Rarity.Common] = new List<FishData>();
        rarityDict[Rarity.Uncommon] = new List<FishData>();
        rarityDict[Rarity.Rare] = new List<FishData>();
        rarityDict[Rarity.Legendary] = new List<FishData>();
        rarityDict[Rarity.Mythical] = new List<FishData>();

        // Sort fish into rarity buckets
        foreach (var fish in fishList)
        {
            if (rarityDict.ContainsKey(fish.rarity))
            {
                rarityDict[fish.rarity].Add(fish);
            }
        }

        rarityPools[zone] = rarityDict;
    }

    public FishData GetFish(DayPeriod dayPeriod, FishData.Zone zone)
    {
        List<FishData> zoneList = GetZoneList(zone);
        return RandomFishByRarity(zoneList);
    }

    public FishData GetFishByZoneAndRarity(DayPeriod dayPeriod, FishData.Zone zone, Rarity rarity)
    {
        // Get precomputed rarity pool
        if (rarityPools == null || !rarityPools.ContainsKey(zone) || !rarityPools[zone].ContainsKey(rarity))
        {
            return null;
        }

        List<FishData> rarityPool = rarityPools[zone][rarity];

        // If no fish at this rarity, return null to trigger reroll
        if (rarityPool.Count == 0)
        {
            return null;
        }

        // Random fish from the rarity pool
        FishData resultantFish = rarityPool[Random.Range(0, rarityPool.Count)];
        if (printDebugStatements) Debug.Log($"{resultantFish.displayName} chosen as caught fish");
        return resultantFish;
    }

    public bool HasFishAtRarity(FishData.Zone zone, Rarity rarity)
    {
        if (rarityPools == null || !rarityPools.ContainsKey(zone) || !rarityPools[zone].ContainsKey(rarity))
        {
            return false;
        }

        return rarityPools[zone][rarity].Count > 0;
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
