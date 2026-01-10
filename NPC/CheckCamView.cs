using UnityEngine;
using NaughtyAttributes;

public class CheckCamView : MonoBehaviour
{
    [SerializeField] Camera convoCam;

    [Button]
    private void SetToPos()
    {
        convoCam.transform.position = transform.position;
        convoCam.transform.rotation = transform.rotation;
    }
}
