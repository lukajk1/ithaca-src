using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingManager : MonoBehaviour
{
    [SerializeField] private AudioClip caughtFishSFX;
    private const float minBiteTime = 6f;
    private const float maxBiteTime = 12f;

    Dictionary<string, float> relativeRarity = new()
    {
        { "common", 100f },
        { "uncommon", 35f },
        { "rare", 8f },
        { "legendary", 0.1f },
        { "mythic", 0f },
    };

    Dictionary<string, float> relativeItemChance = new()
    {
        { "fish", 100f },
        { "scrap", 10f },
    };

    private FishData.Zone fishingZone;

    public static FishingManager i;

    private void Awake()
    {
        i = this;
    }

    public void Fish()
    {
        DayPeriod currentPeriod = DayPeriod.Afternoon;

        Rarity rolledRarity = RollWeightedRarity();
        FishData caughtFish = FishDatabase.i.GetFishByZoneAndRarity(currentPeriod, FishData.Zone.Saltwater, rolledRarity);

        // If no fish found at that rarity, reroll until we find one
        int maxAttempts = 10;
        int attempts = 0;
        while (caughtFish == null && attempts < maxAttempts)
        {
            rolledRarity = RollWeightedRarity();
            caughtFish = FishDatabase.i.GetFishByZoneAndRarity(currentPeriod, FishData.Zone.Saltwater, rolledRarity);
            attempts++;
        }

        // Fallback to old method if all attempts fail
        if (caughtFish == null)
        {
            caughtFish = FishDatabase.i.GetFish(currentPeriod, FishData.Zone.Saltwater);
        }

        StartCoroutine(FishingWaitCR(caughtFish, NormalRandom()));
    }

    private Rarity RollWeightedRarity()
    {
        // Apply player stat modifiers to base rarity weights
        Dictionary<Rarity, float> weightedRarities = new()
        {
            { Rarity.Common, relativeRarity["common"] * PlayerInfo.Stats.commonWeightModifier },
            { Rarity.Uncommon, relativeRarity["uncommon"] * PlayerInfo.Stats.uncommonWeightModifier },
            { Rarity.Rare, relativeRarity["rare"] * PlayerInfo.Stats.rareWeightModifier },
            { Rarity.Legendary, relativeRarity["legendary"] * PlayerInfo.Stats.legendaryWeightModifier },
            { Rarity.Mythical, relativeRarity["mythic"] }
        };

        // Calculate total weight
        float totalWeight = 0;
        foreach (var weight in weightedRarities.Values)
        {
            totalWeight += weight;
        }

        // Roll random value and select rarity
        float random = Random.Range(0, totalWeight);
        float currentWeight = 0;

        foreach (var kvp in weightedRarities)
        {
            currentWeight += kvp.Value;
            if (random < currentWeight)
            {
                return kvp.Key;
            }
        }

        // Fallback to common
        return Rarity.Common;
    }

    private IEnumerator FishingWaitCR(FishData caughtFish, float quality)
    {
        float biteTime = Random.Range(minBiteTime, maxBiteTime) / PlayerInfo.Stats.biteSpeedModifier;
        // Debug.Log("bitetime" + biteTime);
        // Debug.Log("speedmod" + PlayerInfo.Stats.biteSpeedModifier);
        yield return new WaitForSeconds(biteTime);

        //Debug.Log($"Caught a {caughtFish.displayName}!");
        PlayerInfo.TryAddToInventory(new ConcreteItem(caughtFish, quality));
        SoundManager.Play(new SoundData(caughtFishSFX, SoundData.Type.SFX, varyPitch: false, varyVolume: false));
    }

    private float NormalRandom()
    {
        // Central limit theorem - averaging creates normal distribution
        float value = (Random.value + Random.value + Random.value) / 3f;
        value = Mathf.Clamp(value, 0.01f, 0.99f);
        return Mathf.Round(value * 100f) / 100f; // Round to 2 decimal places
    }
}