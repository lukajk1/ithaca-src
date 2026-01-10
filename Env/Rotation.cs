using UnityEngine;

public class Rotation : MonoBehaviour
{
    [Tooltip("If checked, rotation is applied in local space (Space.Self). If unchecked, rotation is applied in global space (Space.World).")]
    public bool useLocalRotation;

    [Tooltip("Degrees of rotation per second for the X, Y, and Z axes.")]
    public Vector3 rotationDegreesPerSecond = new Vector3(0, 0, 0);

    void Update()
    {
        Vector3 rotationStep = rotationDegreesPerSecond * Time.deltaTime;

        Space space = useLocalRotation ? Space.Self : Space.World;

        transform.Rotate(rotationStep, space);
    }
}
