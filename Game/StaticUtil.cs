using UnityEngine;

public static class StaticUtil
{
    public static float RandomInNormalDist(float mean = 0.5f, float stdDev = 0.15f)
    {
        float u1 = 1.0f - Random.value;
        float u2 = 1.0f - Random.value;

        float randStdNormal = Mathf.Sqrt(-2.0f * Mathf.Log(u1)) * Mathf.Sin(2.0f * Mathf.PI * u2);
        float value = mean + stdDev * randStdNormal;

        return Mathf.Clamp01(value);
    }
}