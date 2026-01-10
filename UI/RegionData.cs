using UnityEngine;
public enum Region
{
    Outskirts,
    Lowtown,
    EbuDistrict,
    Highside,
}
public class RegionData : MonoBehaviour
{
    public static string RegionToString(Region region)
    {
        switch (region)
        {
            case Region.Lowtown:
                return "Lowtown";
            case Region.EbuDistrict:
                return "Ebu District";
            case Region.Outskirts:
                return "The Docks";
            case Region.Highside:
                return "Highside";
            default:
                return "Unknown Region";
        }
    }
}
