using UnityEngine;
using NaughtyAttributes;
using System.Collections;

[System.Serializable]
public struct EnvPreset
{
    [ColorUsage(true, true)] public Color ambientLightColor;
    public Color fogColor;
    public Color sunColor;
    public float fogDensity;
    public float sunIntensity;
}

public class WeatherController : MonoBehaviour
{
    [SerializeField] private Light sun;

    // this should not be publicly accessible - requests should be routed through (this) 
    [SerializeField] private RainController rainController;

    [SerializeField] private float defaultSunIntensity;
    [SerializeField] private float defaultFogDensity;

    [Header("daynight cycle")]
    [SerializeField] private EnvPreset blue;
    [SerializeField] private EnvPreset twilight;
    [SerializeField] private EnvPreset morning;
    [SerializeField] private EnvPreset afternoon;
    [SerializeField] private EnvPreset night;

    private const float transitionSpeed = 15f;

    [Header("weather conditions")]

    public static WeatherController i { get; private set; }

    #region setup
    private void Awake()
    {
        if (i != null)
        {
            Debug.LogError("multiple weather controllers in scene");
        }
        i = this;
    }

    void Start()
    {
        OnTimeChanged(GTime.Time);
    }

    private void OnEnable()
    {
        GTime.OnTimeChanged += OnTimeChanged;
    }

    private void OnDisable() { 
        GTime.OnTimeChanged -= OnTimeChanged;
    }

    #endregion

    public void OnTimeChanged((int, int) time)
    {
        int hours = time.Item1;
        int minutes = time.Item2;

        if (hours == 4 && minutes == 00) Transition(blue, transitionSpeed);
        if (hours == 6 && minutes == 00) Transition(twilight, transitionSpeed);
        if (hours == 7 && minutes == 30) Transition(morning, transitionSpeed);
        if (hours == 10 && minutes == 00) Transition(afternoon, transitionSpeed);
        if (hours == 17 && minutes == 30) Transition(twilight, transitionSpeed);
        if (hours == 20 && minutes == 00) Transition(blue, transitionSpeed);
        if (hours == 21 && minutes == 30) Transition(night, transitionSpeed);

    }
    public static bool SetWeather(GWeather weather)
    {
        if (i != null)
        {
            // presumably I will need use of instanced refs.. and using a static call is a little cleaner looking
            // plus if this is called somewhere where there isn't an instance then it's protected from a nullref
            return true;
        }
        return false;
    }

    public void SetRain(bool value)
    {
        rainController.SetRain(value);  
    }
    
    [Button]
    private void ToggleSun()
    {
        sun.enabled = !sun.enabled;
    }

    private Coroutine transitionRoutine;

    private void Transition(EnvPreset preset, float transitionLength)
    {
        if (transitionRoutine != null)
            StopCoroutine(transitionRoutine);

        transitionRoutine = StartCoroutine(TransitionRoutine(preset, transitionLength));
    }

    private IEnumerator TransitionRoutine(EnvPreset target, float duration)
    {
        Color startFogColor = RenderSettings.fogColor;
        Color startAmbient = RenderSettings.ambientLight;
        Color startSunColor = sun.color;

        float startFogDensity = RenderSettings.fogDensity;
        float startSunIntensity = sun.intensity;

        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float lerpT = Mathf.Clamp01(t / duration);

            RenderSettings.fogColor = Color.Lerp(startFogColor, target.fogColor, lerpT);
            RenderSettings.ambientLight = Color.Lerp(startAmbient, target.ambientLightColor, lerpT);
            sun.color = Color.Lerp(startSunColor, target.sunColor, lerpT);

            float targetFog = target.fogDensity == -1 ? defaultFogDensity : target.fogDensity;
            float targetIntensity = target.sunIntensity == -1 ? defaultSunIntensity : target.sunIntensity;

            RenderSettings.fogDensity = Mathf.Lerp(startFogDensity, targetFog, lerpT);
            sun.intensity = Mathf.Lerp(startSunIntensity, targetIntensity, lerpT);

            yield return null;
        }

        // ensure final values are applied exactly
        SetEnv(target);
        transitionRoutine = null;
    }


    private void SetEnv(EnvPreset preset)
    {
        RenderSettings.fogColor = preset.fogColor;
        RenderSettings.ambientLight = preset.ambientLightColor;
        sun.color = preset.sunColor;

        RenderSettings.fogDensity = preset.fogDensity == -1 ? defaultFogDensity : preset.fogDensity;
        sun.intensity = preset.sunIntensity == -1 ? defaultSunIntensity : preset.sunIntensity;
    }
}