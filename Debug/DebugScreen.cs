using UnityEngine;

public class DebugScreen : MonoBehaviour
{
    [SerializeField] private Canvas canvas;

    void Start()
    {
        canvas.gameObject.SetActive(false);
    }


}
