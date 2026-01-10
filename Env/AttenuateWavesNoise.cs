using UnityEngine;

public class AttenuateWavesNoise : MonoBehaviour
{
    [SerializeField] private float floorYHeight = 15f;
    [SerializeField] private float ceilingYHeight = 100f;

    [SerializeField] private float ceilingFreqCutoff = 750f;

    private AudioHighPassFilter highPass;
    private AudioSource wavesSource;
    private float wavesVolume;

    private void Awake()
    {
        highPass = GetComponent<AudioHighPassFilter>();
        wavesSource = GetComponent<AudioSource>();
        wavesVolume = wavesSource.volume;
    }

    private void Update()
    {
        float t = Mathf.InverseLerp(floorYHeight, ceilingYHeight, transform.position.y);
        t = Mathf.Clamp01(t);
        float inverted = 1f - t; // highest at low y and lowest at high y

        wavesSource.volume = Mathf.Lerp(0f, wavesVolume, inverted);
        highPass.cutoffFrequency = Mathf.Lerp(0f, ceilingFreqCutoff, t);
    }
}
