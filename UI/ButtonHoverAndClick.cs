using UnityEngine;
using UnityEngine.EventSystems;
using NaughtyAttributes;
using UnityEngine.UI;

public class ButtonHoverAndClick : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private AudioClip onHoverClip;

    [SerializeField] private bool useClickSFX;
    [ShowIf("useClickSFX")]
    [SerializeField] private AudioClip onClickSFX;

    private Button button;
    void Awake()
    {
        button = GetComponent<Button>();
    }

    void OnEnable()
    {
        button.onClick.AddListener(OnClick);
    }

    void OnDisable()
    {
        button.onClick.RemoveListener(OnClick);
    }

    private void OnClick()
    {
        if (useClickSFX) SoundManager.Play(new SoundData(onClickSFX, SoundData.Type.SFX));
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        SoundManager.Play(new SoundData(onHoverClip, SoundData.Type.SFX));
    }

    public void OnPointerExit(PointerEventData eventData) { }
}
