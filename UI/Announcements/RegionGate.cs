using UnityEngine;

public class RegionGate : AInteractable
{
    [SerializeField] private Region region1;
    [SerializeField] private Region region2;
    public override void Interact()
    {
        Region enteringRegion = region1;

        if (PlayerInfo.currentRegion == region1)
        {
            enteringRegion = region2;
        }

        PlayerInfo.currentRegion = enteringRegion;

        Announcements.i.ShowRegionTitle(RegionData.RegionToString(enteringRegion).ToUpper());
    }
}
