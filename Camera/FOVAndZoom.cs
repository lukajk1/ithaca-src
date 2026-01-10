using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class FOVAndZoom : MonoBehaviour
{
    [SerializeField] InputActionReference zoom;
    private Camera cam;
    private const float zoomValue = 28f;
    private float normal;
    private bool _isZoomed;

    public bool IsZoomed
    {
        get { return _isZoomed; }
        set
        {
            if (value && !_isZoomed)
            {
                SetFOV(zoomValue);
                _isZoomed = value;
            }
            else if (!value && _isZoomed)
            {
                SetFOV(normal);
                _isZoomed = value;
            }
        }
    }

    private void OnEnable()
    {
        zoom.action.Enable();
        zoom.action.performed += OnZoomPressed;
        zoom.action.canceled += OnZoomReleased;
    }

    private void OnDisable()
    {
        zoom.action.Disable();
        zoom.action.performed -= OnZoomPressed;
        zoom.action.canceled -= OnZoomReleased;
    }

    void Start()
    {
        cam = GetComponent<Camera>();
        normal = cam.fieldOfView;
    }

    public void SetFOV(float fov)
    {
        cam.fieldOfView = fov;
    }

    private void OnZoomPressed(InputAction.CallbackContext context)
    {
        IsZoomed = true;
    }

    private void OnZoomReleased(InputAction.CallbackContext context)
    {
        IsZoomed = false;
    }
}