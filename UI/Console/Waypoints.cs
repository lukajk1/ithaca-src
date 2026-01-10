using UnityEngine;

public class Waypoints : MonoBehaviour
{
    [SerializeField] Transform dock;
    [SerializeField] Transform bar;
    [SerializeField] Transform overpass;

    public Transform GetWaypoint(string location)
    {
        switch (location)
        {
            case "dock":
                return dock;
            case "bar":
                return bar;
            case "road": 
                return overpass;
            default:
                return null;
        }
    }
}
