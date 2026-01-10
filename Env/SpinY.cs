using UnityEngine;
public class SpinY : MonoBehaviour
{
    [SerializeField] private float degreesPerSecond = 15f;

    void Update()
    {
        transform.Rotate(0f, 0f, degreesPerSecond * Time.deltaTime);
    }
}
