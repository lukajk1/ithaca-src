using UnityEngine;

public class ShowDefaultCanvas : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    private void Awake()
    {
        canvas.gameObject.SetActive(true);
    }
}
