using UnityEngine;

public class SellItem : MonoBehaviour
{
    [SerializeField] private AnimationCurve priceQualityRatio;

    private static SellItem i;
    private void Awake()
    {
        i = this;
    }

    public static int GetPrice(ConcreteItem item)
    {
        if (item.data is FishData)
        {
            return Mathf.RoundToInt(i.priceQualityRatio.Evaluate(item.quality) * item.data.value);
            //Debug.Log($"{item.data.displayName} @ quality {item.quality} sells for {salePrice}");
        }
        else
        {
            return item.data.value;
        }
        
    } 
}
