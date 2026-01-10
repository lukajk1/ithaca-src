using TMPro;
using UnityEngine;

public class TooltipInner : MonoBehaviour
{
    [SerializeField] private Vector2 cursorOffset = new Vector2(20f, -20f);

    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI subheadingText;
    [SerializeField] private TextMeshProUGUI qualityText;
    [SerializeField] private TextMeshProUGUI flavorText;
    [SerializeField] private GameObject rightClickToEquipNotice;
    [SerializeField] private TextMeshProUGUI sellValue;

    private RectTransform rectTransform;
    private Canvas canvas;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        sellValue.gameObject.SetActive(false);
    }

    public void SetContent(ConcreteItem item)
    {
        if (item == null) return;

        titleText.text = item.data.displayName;
        titleText.color = ColorLookup.GetColor(item.data.rarity);


        // subheading
        subheadingText.gameObject.SetActive(true); // subheading may be hidden by other setcontent() overloads so set it as active here
        if (item.data is FishData fishData)
        {
            subheadingText.text = $"{fishData.rarity.ToString().ToLower()} {fishData.family.ToString().ToLower()}";
        }
        else
        {
            subheadingText.text = $"{item.data.rarity.ToString().ToLower()} {item.data.type.ToString().ToLower()}";
        }

        // quality
        qualityText.gameObject.SetActive(item.data is FishData);

        // 1. Get the color object from the gradient evaluation
        Color qualityColor = Tooltip.i.qualityGradient.Evaluate(item.quality);

        // 2. Convert the Color object to an RGB hex string (e.g., "FF00A8")
        // We use ToHtmlStringRGB to get the hex code without the leading "#" sign.
        string hexColor = ColorUtility.ToHtmlStringRGB(qualityColor);

        // 3. Construct the text using the color tag around the dynamic value
        qualityText.text = $"quality: <color=#{hexColor}>{item.quality.ToString("F2")}</color>";


        // flavor
        flavorText.gameObject.SetActive(item.selectedFlavorText != null);
        if (item.selectedFlavorText != null) flavorText.text = item.selectedFlavorText;

        // equip notice
        bool showEquip = item.data is EquipmentData && Game.context == Game.MenuContext.Inventory;
        rightClickToEquipNotice.SetActive(showEquip);

        // sell value
        bool isShop = Game.context == Game.MenuContext.Shop;
        sellValue.gameObject.SetActive(isShop);

        if (isShop)
        {
            sellValue.text = $"sell value: {SellItem.GetPrice(item)}";
        }
    }

    public void SetContent(string title, string subheading, string body)
    {
        titleText.text = title;
        if (subheading == null)
        {
            subheadingText.gameObject.SetActive(false); 
        }
        else
        {
            subheadingText.text = subheading;
        }
        flavorText.text = body;   
    }

    private void Update()
    {
        // Assuming:
        // - rectTransform.pivot is set to (0, 1) [Top-Left]
        // - rectTransform.anchor is set to (0.5, 0.5) [Center]

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvas.transform as RectTransform,
            Input.mousePosition,
            canvas.renderMode == RenderMode.ScreenSpaceOverlay ? null : canvas.worldCamera,
            out localPoint);

        // localPoint is currently relative to the canvas CENTER.
        localPoint += cursorOffset;

        // *** REVISED CLAMPING LOGIC ***

        float tooltipWidth = rectTransform.rect.width;
        float tooltipHeight = rectTransform.rect.height;
        float canvasWidth = (canvas.transform as RectTransform).rect.width;
        float canvasHeight = (canvas.transform as RectTransform).rect.height;

        // Convert localPoint from Canvas Center-based coordinates to Canvas Top-Left-based coordinates
        // Canvas Center is (0, 0). Canvas Top-Left is (-canvasWidth/2, canvasHeight/2).
        // The pivot being (0, 1) means we clamp the Top-Left corner (0, 1) of the tooltip.

        // X-axis: Clamp the left edge (localPoint.x) between the left canvas edge and 
        //         the right canvas edge MINUS the tooltip width.
        localPoint.x = Mathf.Clamp(localPoint.x, -canvasWidth / 2f, canvasWidth / 2f - tooltipWidth);

        // Y-axis: Clamp the top edge (localPoint.y) between the top canvas edge and 
        //         the bottom canvas edge PLUS the tooltip height (since Y is negative below center).
        localPoint.y = Mathf.Clamp(localPoint.y, -canvasHeight / 2f + tooltipHeight, canvasHeight / 2f);

        rectTransform.anchoredPosition = localPoint;
    }
}
