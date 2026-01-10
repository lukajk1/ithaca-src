//using DistantLands.Cozy;
//using TMPro;
//using UnityEngine;

//public class CozyWeatherClock : MonoBehaviour
//{
//    [SerializeField] TextMeshProUGUI clock;
//    CozyTimeModule timeModule;

//    // Stores the minute the clock was last updated to prevent constant updates
//    private int lastDisplayedMinute = -1;

//    private void Start()
//    {
//        timeModule = CozyWeather.instance.timeModule;
//        // Force an initial update immediately when the game starts
//        UpdateClockDisplay(forceUpdate: true);
//    }

//    private void Update()
//    {
//        // Check if the clock should be updated (i.e., every 10 minutes)
//        UpdateClockDisplay();
//    }

//    private void UpdateClockDisplay(bool forceUpdate = false)
//    {
//        // 1. Get the current minutes and hours
//        int rawHours24 = Mathf.FloorToInt(timeModule.currentTime.hours);
//        int currentMinutes = Mathf.FloorToInt(timeModule.currentTime.minutes);

//        // --- Update Logic ---
//        // Check if the current minute is a multiple of 10 AND
//        // if this minute hasn't been displayed yet (to avoid updating every frame for one minute)
//        bool minuteChanged = currentMinutes != lastDisplayedMinute;
//        bool isTenMinuteInterval = currentMinutes % 10 == 0;

//        if (!forceUpdate && (!minuteChanged || !isTenMinuteInterval))
//        {
//            return; // Exit if we shouldn't update yet
//        }

//        // The time has reached a new 10-minute interval, so update the display.
//        lastDisplayedMinute = currentMinutes;

//        // 2. Convert to 12-hour format and determine AM/PM
//        int hours12 = rawHours24;
//        string amPm;

//        if (hours12 >= 12)
//        {
//            amPm = "PM";
//            if (hours12 > 12)
//            {
//                // Subtract 12 for PM times (e.g., 13 -> 1)
//                hours12 -= 12;
//            }
//        }
//        else // hours12 is < 12 (AM)
//        {
//            amPm = "AM";
//            if (hours12 == 0) // Convert 0 (midnight) to 12 AM
//            {
//                hours12 = 12;
//            }
//        }

//        // 3. Format the time string
//        // :D2 ensures minutes are always two digits (e.g., 05 instead of 5)
//        string timeString = $"{hours12}:{currentMinutes:D2} {amPm}";

//        // 4. Update the TextMeshPro UI
//        if (clock != null)
//        {
//            clock.text = timeString;
//        }
//    }
//}