using UnityEngine;
using NaughtyAttributes;
using System.Collections;

[System.Serializable]
public struct WeatherData
{
    [ColorUsage(true, true)] public Color ambientLightColor;
    public Color fogColor;
    public Color sunColor;
    public float fogDensity;
    public float sunIntensity;
}
