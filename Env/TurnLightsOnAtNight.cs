using System.Collections.Generic;
using UnityEngine;

public class TurnLightsOnAtNight : MonoBehaviour
{
    [SerializeField] public List<Material> lightsList;

    private void OnEnable()
    {
        GTime.OnTimeChanged += OnTimeChanged;
    }

    private void OnDisable()
    {
        GTime.OnTimeChanged -= OnTimeChanged;
    }

    private void OnTimeChanged((int, int) time)
    {
        if (time.Item1 == 19 && time.Item2 == 00)
        {
            SetEnabled(true);
        }
        else if (time.Item1 == 7 && time.Item2 == 00)
        {
            SetEnabled(false);
        }
    }

    private void SetEnabled(bool enabled)
    {
        foreach (Material mat in lightsList)
        {
            if (enabled)
            {
                if (mat.HasProperty("_EmissionColor"))
                {
                    mat.EnableKeyword("_EMISSION");
                }
            }
            else
            {
                if (mat.HasProperty("_EmissionColor"))
                {
                    mat.DisableKeyword("_EMISSION");
                }
            }

        }

    }
}