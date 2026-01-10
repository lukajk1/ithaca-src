using UnityEngine;

[RequireComponent(typeof(Light))]
public class HoloLightFlicker : MonoBehaviour
{
    // The maximum amount the intensity can deviate from its original value.
    public float flickerMargin = 0.5f;

    // The speed at which the intensity changes over time.
    [Range(0.1f, 10f)]
    public float flickerSpeed = 5f;

    private Light targetLight;
    private float baseIntensity;
    private float timeOffset;

    void Start()
    {
        targetLight = GetComponent<Light>();

        // Store the original intensity to calculate the flicker offset from.
        baseIntensity = targetLight.intensity;

        // Use a random offset for the Perlin noise to make multiple lights flicker independently.
        timeOffset = Random.Range(0f, 100f);
    }

    void Update()
    {
        // Calculate the current time position based on speed and offset.
        float time = Time.time * flickerSpeed + timeOffset;

        // Get a smooth, random-like value (0 to 1) from Perlin noise.
        float noise = Mathf.PerlinNoise(time, time);

        // Map the noise value to the intensity range around the base intensity:
        // (noise * 2 - 1) converts the 0-1 range to a -1 to 1 range.
        float flickerDelta = (noise * 2f - 1f) * flickerMargin;

        // Apply the flicker delta to the base intensity.
        targetLight.intensity = baseIntensity + flickerDelta;
    }
}
