using UnityEngine;
using UnityEngine.Rendering;

public class Tooltip : MonoBehaviour
{
    public static Tooltip i;
    [SerializeField] private TooltipInner tooltip;
    [SerializeField] private Canvas canvas;
    [SerializeField] public Gradient qualityGradient;
    private CanvasGroup cg;

    private void Awake()
    {
        i = this;
        cg = i.tooltip.gameObject.GetComponent<CanvasGroup>();

        Hide();
    }

    private void Update()
    {
        if (Game.context == Game.MenuContext.None) i.canvas.gameObject.SetActive(false);
    }

    public static void Show(ConcreteItem item)
    {
        i.tooltip.SetContent(item);
        i.canvas.gameObject.SetActive(true);
        LeanTween.alphaCanvas(i.cg, 1, 0.14f);
    }

    public static void Show(string title, string subheading, string body)
    {
        i.tooltip.SetContent(title, subheading, body);
        i.canvas.gameObject.SetActive(true);
        LeanTween.alphaCanvas(i.cg, 1, 0.14f);
    }

    public static void Hide()
    {
        i.canvas.gameObject.SetActive(false);
        LeanTween.alphaCanvas(i.cg, 0, 0);
    }
}
