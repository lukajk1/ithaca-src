using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLookRotation : MonoBehaviour
{
    [Header("Input")]
    public InputActionReference mouseDeltaAction;

    [Header("Rotation Settings")]
    public float mouseSensitivity = 100f;
    public bool invertY = false;

    [Header("Rotation Limits")]
    public bool clampVerticalRotation = true;
    public float minVerticalAngle = -90f;
    public float maxVerticalAngle = 90f;

    // Current rotation values
    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        // Initialize rotation values based on current transform rotation
        Vector3 currentRotation = transform.eulerAngles;
        rotationY = currentRotation.y;
        rotationX = currentRotation.x;

        // Handle the case where X rotation might be > 180 (Unity's angle representation)
        if (rotationX > 180f)
            rotationX -= 360f;

        // Enable the input action
        if (mouseDeltaAction != null)
        {
            mouseDeltaAction.action.Enable();
        }
    }

    void OnDisable()
    {
        // Disable the input action when the component is disabled
        if (mouseDeltaAction != null)
        {
            mouseDeltaAction.action.Disable();
        }
    }

    void Update()
    {
        ApplyRotation();
    }

    void ApplyRotation()
    {
        // Read mouse delta from the input action reference
        Vector2 mouseDelta = Vector2.zero;
        if (mouseDeltaAction != null)
        {
            mouseDelta = mouseDeltaAction.action.ReadValue<Vector2>();
        }

        // Apply sensitivity and frame-rate independence
        float mouseX = mouseDelta.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseDelta.y * mouseSensitivity * Time.deltaTime;

        // Invert Y if needed
        if (invertY)
            mouseY = -mouseY;

        // Update rotation values
        rotationY += mouseX;  // Y-axis rotation (horizontal mouse movement)
        rotationX -= mouseY;  // X-axis rotation (vertical mouse movement, inverted)

        // Clamp vertical rotation if enabled
        if (clampVerticalRotation)
        {
            rotationX = Mathf.Clamp(rotationX, minVerticalAngle, maxVerticalAngle);
        }

        // Apply the rotation to the transform
        transform.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
    }

    // Optional: Reset rotation method
    public void ResetRotation()
    {
        rotationX = 0f;
        rotationY = 0f;
        transform.rotation = Quaternion.identity;
    }
}