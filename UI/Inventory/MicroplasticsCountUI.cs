using UnityEngine;
using UnityEngine.EventSystems;

public class MicroplasticsCountUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AudioClip hover;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        Tooltip.Show(
            "Microplastics",
            null,
            "The universal currency. Some citizens are fond of the taste."
            );
        SoundManager.Play(new SoundData(hover, SoundData.Type.SFX));
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.Hide();

    }

}