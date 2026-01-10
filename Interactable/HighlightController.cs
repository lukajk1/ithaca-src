using HighlightPlus;
using NaughtyAttributes;
using UnityEngine;

public class HighlightController : MonoBehaviour
{
    HighlightEffect effect;

    void Awake()
    {
        effect = GetComponent<HighlightEffect>();
    }

    [Button]
    public void ToggleHighlight()
    {
        effect.highlighted = !effect.highlighted;
    } 
    public void SetHighlight(bool value)
    {
        effect.highlighted = value;
    }
}
