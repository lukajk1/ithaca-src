using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct FishRecord
{
    public int timesCaught;
    public float highestQuality;

    public FishRecord(int timesCaught, float highestQuality)
    {
        this.timesCaught = timesCaught;
        this.highestQuality = highestQuality;
    }
}

[System.Serializable]
public struct FishDataRecordPair
{
    public FishData fishData;
    public FishRecord record;
}

public class FishDiary : MonoBehaviour
{
    // Serialized data (saved to disk/inspector)
    [SerializeField] private List<FishDataRecordPair> savedRecords = new List<FishDataRecordPair>();

    // Runtime dictionary (fast lookups)
    private static Dictionary<FishData, FishRecord> fishRecords = new Dictionary<FishData, FishRecord>();

    public static FishDiary i;

    private void Awake()
    {
        i = this;
        LoadRecordsFromSerialized();
    }

    private void LoadRecordsFromSerialized()
    {
        fishRecords.Clear();
        foreach (var pair in savedRecords)
        {
            if (pair.fishData != null)
            {
                fishRecords[pair.fishData] = pair.record;
            }
        }
    }

    private void SaveRecordsToSerialized()
    {
        savedRecords.Clear();
        foreach (var kvp in fishRecords)
        {
            savedRecords.Add(new FishDataRecordPair
            {
                fishData = kvp.Key,
                record = kvp.Value
            });
        }
    }

    public static void Record(ConcreteItem caughtFish)
    {
        if (caughtFish == null || caughtFish.data == null || !(caughtFish.data is FishData))
        {
            Debug.LogWarning("Attempted to record invalid fish");
            return;
        }

        FishData fishData = caughtFish.data as FishData;
        float quality = caughtFish.quality;

        if (fishRecords.ContainsKey(fishData))
        {
            // Update existing record
            FishRecord existingRecord = fishRecords[fishData];
            existingRecord.timesCaught++;
            existingRecord.highestQuality = Mathf.Max(existingRecord.highestQuality, quality);
            fishRecords[fishData] = existingRecord;
        }
        else
        {
            // Create new record
            fishRecords[fishData] = new FishRecord(1, quality);
        }

        // Update serialized data
        if (i != null)
        {
            i.SaveRecordsToSerialized();
        }
    }

    public static FishRecord? GetRecord(FishData fishData)
    {
        if (fishRecords.ContainsKey(fishData))
        {
            return fishRecords[fishData];
        }
        return null;
    }

    public static bool HasCaught(FishData fishData)
    {
        return fishRecords.ContainsKey(fishData);
    }

    public static Dictionary<FishData, FishRecord> GetAllRecords()
    {
        return new Dictionary<FishData, FishRecord>(fishRecords);
    }

    public static int GetTotalFishCaught()
    {
        int total = 0;
        foreach (var record in fishRecords.Values)
        {
            total += record.timesCaught;
        }
        return total;
    }

    public static int GetUniqueFishCaught()
    {
        return fishRecords.Count;
    }

    private void OnDestroy()
    {
        SaveRecordsToSerialized();
    }
}
