using UnityEngine;

public class GroundedHitbox : MonoBehaviour
{

    private int objectsCount;
    [HideInInspector] public ObjectMaterial.Type materialType;

    private void OnTriggerEnter(Collider other)
    {
        objectsCount++;

        ObjectMaterial mat = other.gameObject.GetComponent<ObjectMaterial>();
        if (mat != null)
        {
            materialType = mat.type;
        }
        else materialType = ObjectMaterial.Type.Default;
    }

    private void OnTriggerExit(Collider other)
    {
        objectsCount--;
    }
    private void Update()
    {
        //Debug.Log(IsGrounded());
    }

    public bool IsGrounded() => objectsCount > 0;
}
