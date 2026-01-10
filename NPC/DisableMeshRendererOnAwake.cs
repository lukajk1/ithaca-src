using UnityEngine;

public class DisableMeshRendererOnAwake : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<MeshRenderer>().enabled = false;
    }
}
