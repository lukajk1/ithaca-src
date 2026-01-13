using QFSW.QC;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CaughtFishAlert : MonoBehaviour
{
    [SerializeField] private Button confirmButton;
    [SerializeField] private SlideTransition transition;

    [SerializeField] private Image fishDisplay;
    [SerializeField] private TextMeshProUGUI fishName;
    [SerializeField] private TextMeshProUGUI fishQualityFloat;
    [SerializeField] private TextMeshProUGUI fishFlavorText;

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
    void DebugDisplay()
    {
        LockActionMap.i.ModifyLockList(true, this);
        Game.ModifyCursorUnlockList(true, this);
        i.transition.Animate(true);
    }
    public void Display(ConcreteItem caughtFish)
    {
        i.fishDisplay.sprite = caughtFish.data.icon;
        i.fishName.text = caughtFish.data.displayName;
        i.fishFlavorText.text = caughtFish.selectedFlavorText;

        Color qualityColor = Tooltip.i.qualityGradient.Evaluate(caughtFish.quality);
        string hexColor = ColorUtility.ToHtmlStringRGB(qualityColor);

        i.fishQualityFloat.text = $"quality: <color=#{hexColor}>{caughtFish.quality}</color>";

        LockActionMap.i.ModifyLockList(true, this);
        Game.ModifyCursorUnlockList(true, this);
        i.transition.Animate(true);
    }

    void Close()
    {
        transition.Animate(false);
        LockActionMap.i.ModifyLockList(false, this);
        Game.ModifyCursorUnlockList(false, this);

    }
}
