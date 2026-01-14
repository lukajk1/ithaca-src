using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewFish", menuName = "Ithaca/Fish")]
public class FishData : ItemData
{
    public override Type type
    {
        get => Type.Creature;
        set { } // Ignore attempts to set
    }

    [HorizontalLine(color: EColor.Blue)]
    [Header("Fish Properties")]
    public string scientificName;

    public float weightedRarity = 100f;
    
    public Vector2 weightRangeKilograms;
    public Vector2 lengthRangeMeters;


    public List<DayPeriod> cantBeCaughtTimes;

    public Zone zone; 
    public Family family; 
    
    public enum Zone
    {
        Freshwater,
        Saltwater,
        Deep, // fishing in saltwater at night will yield deep fish
        Wetlands,
    }

    public enum Family
    {
        Pisces,
        Crustacean,
        Cetacean,
        Mollusk,
    }

}


