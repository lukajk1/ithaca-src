using UnityEngine;
using NaughtyAttributes;
using System.Collections;

[CreateAssetMenu(fileName = "NewWeather", menuName = "Ithaca/WeatherProfile")]
public class WeatherProfile : ScriptableObject
{
    [ColorUsage(true, true)] public Color ambientLightColor;
    public Color fogColor;
    public Color sunColor;
    public float fogDensity;
    public float sunIntensity;

    public bool hasRain;
    [ShowIf("hasRain")] public RainProfile rainProfile;
}
