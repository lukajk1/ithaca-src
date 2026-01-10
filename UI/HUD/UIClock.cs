using TMPro;
using UnityEngine;

public class UIClock : MonoBehaviour 
{
    [SerializeField] private TextMeshProUGUI clock;
    private void OnEnable()
    {
        GTime.OnTimeChanged += OnTimeChanged;
    }

    private void OnDisable()
    {
        GTime.OnTimeChanged -= OnTimeChanged;
    }

    void OnTimeChanged((int, int) time)
    {
        int hour = time.Item1;
        int minutes = time.Item2;
        if (minutes % 10 != 0) return;

        string suffix = hour >= 12 ? "pm" : "am";
        int hour12 = hour % 12;
        if (hour12 == 0) hour12 = 12;
        
        clock.text = $"{hour12}:{minutes:D2} {suffix}";
    }

}
