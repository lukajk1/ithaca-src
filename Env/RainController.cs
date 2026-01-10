using UnityEngine;

public class RainController : MonoBehaviour
{
    [SerializeField] ParticleSystem rainObject;
    public void SetRain(bool value)
    {
        rainObject.gameObject.SetActive(value);
    }
}
