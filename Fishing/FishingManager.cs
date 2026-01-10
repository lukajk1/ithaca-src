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

        { "scrap", 0f },  
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

        // (this) should query the inventory to figure out what bonuses and effects need to be applied, then simply queries the database with all the relevant information 

        FishData caughtFish = FishDatabase.i.GetFish(currentPeriod, FishData.Zone.Saltwater);

        StartCoroutine(FishingWaitCR(caughtFish, NormalRandom()));
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