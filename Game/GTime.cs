using System;
using System.Collections;
using UnityEngine;

public enum DayPeriod
{
    Dawn,
    Morning,
    Afternoon,
    Evening,
    Night
}
public class GTime : MonoBehaviour
{
    public static bool doTimeTick = true;

    public static (int hours, int minutes) Time; // i wonder if a struct is better.. more readable at least..
    public static Action<(int hours, int minutes)> OnTimeChanged;
    public static Action OnIsDayChanged;

    public static DayPeriod currentDayPeriod;

    public static readonly int dayStartHour = 6;
    public static readonly int dayEndHour = 19;

    public static float dayLengthInMinutes = 15f;
    public static float nightLengthInMinutes = 7f;

    private static float origDayLength, origNightLength;

    private static bool _isDay;
    public static bool isDay
    {
        get =>_isDay; 
        set
        {
            if (_isDay != value)
            {
                _isDay = value;
                OnIsDayChanged?.Invoke();
            }
        }
    }
    private float SecondsPerGameMinute
    {
        get
        {
            float periodLength = isDay ? dayLengthInMinutes : nightLengthInMinutes;
            return (periodLength * 60f) / 720f; // 720 game minutes per 12-hour day or night
        }
    }

    private void Start() 
    {
        origDayLength = dayLengthInMinutes;
        origNightLength = nightLengthInMinutes;

        // starting in start allows other scripts to subscribe in OnEnable() which runs before start()
        SetTime(5, 30);
        StartCoroutine(TimeTick());
    }
    public static void ResetDayNightLengths()
    {
        dayLengthInMinutes = origDayLength;
        nightLengthInMinutes = origNightLength;
    }
    public static void SetTime(int hours, int minutes)
    {
        Time = (hours % 24, minutes % 60);
        OnTimeChanged?.Invoke(Time);
        isDay = Time.hours >= dayStartHour && Time.hours < dayEndHour;

        WeatherController.i.OnTimeChanged(Time);
    }

    private IEnumerator TimeTick()
    {
        while (true)
        {
            if (doTimeTick)
            {
                yield return new WaitForSeconds(SecondsPerGameMinute);
                AddMinutes(1);
            }
            else
            {
                yield return null;
            }
        }
    }

    private void AddMinutes(int delta)
    {
        int totalMinutes = Time.hours * 60 + Time.minutes + delta;
        int hours = (totalMinutes / 60) % 24;
        int minutes = totalMinutes % 60;
        SetTime(hours, minutes);
    }
}
