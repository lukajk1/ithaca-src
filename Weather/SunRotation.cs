using System.Collections;
using System.Timers;
using UnityEngine;

public class SunRotation : MonoBehaviour
{
    private Light sun;

    void Awake()
    {
        sun = GetComponent<Light>();
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }

    private void OnEnable()
    {
        GTime.OnIsDayChanged += OnIsDayChanged;
        GTime.OnTimeChanged += OnTimeChanged;
        GWeather.OnWeatherChanged += OnWeatherChanged;
    }

    private void OnDisable()
    {
        GTime.OnIsDayChanged -= OnIsDayChanged;
        GTime.OnTimeChanged -= OnTimeChanged;
        GWeather.OnWeatherChanged -= OnWeatherChanged;
    }


    void OnIsDayChanged()
    {
    }

    private void OnWeatherChanged(Weather weather)
    {
    }

    void OnTimeChanged((int, int) time)
    {
        if (time.Item2 == 0)
        {
            int daySlice = GTime.dayEndHour - GTime.dayStartHour;
            float dayMinutesLength = daySlice * 60;

            float elapsed = time.Item1 * 60 + time.Item2;
            elapsed -= GTime.dayStartHour * 60f;

            float t = elapsed / dayMinutesLength;

            float xRotation = Mathf.Lerp(0f, 180f, t);
            transform.rotation = Quaternion.Euler(xRotation, 0f, 0f);
        }
    }


    private IEnumerator LerpSun()
    {
        float dayLength = GTime.dayLengthInMinutes * 60f;
        float elapsed = 0;

        while (elapsed < dayLength)
        {
            elapsed += Time.deltaTime;

            float xRotation = Mathf.Lerp(0f, 180f, elapsed / dayLength);
            transform.rotation = Quaternion.Euler(xRotation, 0f, 0f);

            yield return null;
        }
    }

}
