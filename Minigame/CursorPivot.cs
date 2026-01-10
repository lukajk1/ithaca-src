using System.Drawing;
using UnityEngine;

public class CursorPivot : MonoBehaviour
{
    private float rotationSpeed = 90f; // Use degrees per second

    void Update()
    {
        // Rotate right when the Right Arrow key is held down
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }

        // Rotate left when the Left Arrow key is held down
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, 0, -rotationSpeed * Time.deltaTime);
        }
    }
}
