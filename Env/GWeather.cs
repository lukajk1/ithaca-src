using System;
using UnityEngine;

public enum Weather
{
    Sunny,
    Overcast,
    LightRain,
    Rain
}
public class GWeather : MonoBehaviour
{
    public static Action<Weather> OnWeatherChanged;
    private static Weather _currentWeather;
    public static Weather currentWeather
    {
        get => _currentWeather;
        set
        {
            if (_currentWeather != value)
            {
                _currentWeather = value;
                OnWeatherChanged?.Invoke(value);
            }
        }
    }
}
