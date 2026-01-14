using UnityEngine;
using QFSW.QC;

public class FishingMinigame : MonoBehaviour
{
    [SerializeField] private Gradient gradient;
    [SerializeField] private Camera minigameCamera;

    private ConcreteItem caughtFish;

    public static FishingMinigame i;
    private void Awake()
    {
        i = this;

        minigameCamera.gameObject.SetActive(false);
        QuantumRegistry.RegisterObject(this);
    }
    public void Begin(ConcreteItem fish)
    {
        caughtFish = fish;
        minigameCamera.gameObject.SetActive(true);
    }

    [Command("end-minigame")]
    public void Close()
    {
        minigameCamera.gameObject.SetActive(false);
        FishingManager.i.OnComplete(caughtFish);
    }
    void MyMethod()
    {
        Color lineColor = gradient.Evaluate(0.5f);
    }
}
