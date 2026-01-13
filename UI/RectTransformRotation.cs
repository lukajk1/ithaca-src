using UnityEngine;

public class RectTransformRotation : MonoBehaviour
{
    [SerializeField] private Vector3 rotationDegPerSecond = Vector3.zero;

    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (rectTransform != null)
        {
            rectTransform.Rotate(rotationDegPerSecond * Time.deltaTime);
        }
    }
}
