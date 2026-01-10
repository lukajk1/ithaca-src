using UnityEngine;

public class FishingMinigame : MonoBehaviour
{
    [SerializeField] private Gradient gradient;
    [SerializeField] private Camera minigameCamera;

    public static FishingMinigame i;
    private void Awake()
    {
        i = this;

        minigameCamera.gameObject.SetActive(false);  
    }
    public void StartFishing()
    {
        minigameCamera.gameObject.SetActive(true);
    }

    void MyMethod()
    {
        Color lineColor = gradient.Evaluate(0.5f);
    }
}
