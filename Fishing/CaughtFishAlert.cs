using QFSW.QC;
using UnityEngine;
using UnityEngine.UI;

public class CaughtFishAlert : MonoBehaviour
{
    [SerializeField] private Button confirmButton;
    [SerializeField] private SlideTransition transition;
    [SerializeField] private Image fishDisplay;

    public static CaughtFishAlert i;
    private void Awake()
    {
        i = this;

        confirmButton.onClick.AddListener(Close);
    }

    private void Start()
    {
        QuantumRegistry.RegisterObject<CaughtFishAlert>(this);
    }

    [Command("caught-fish-display")]
    private void Display()
    {
        //i.fishDisplay.sprite = caughtFish.icon;
        LockActionMap.i.Lock(true);
        Game.ModifyCursorUnlockList(true, this);
        i.transition.Animate(true);
    }

    void Close()
    {
        transition.Animate(false);
        LockActionMap.i.Lock(false);
        Game.ModifyCursorUnlockList(false, this);

    }
}
