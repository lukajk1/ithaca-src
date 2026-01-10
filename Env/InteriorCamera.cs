using NaughtyAttributes;
using UnityEngine;

public class InteriorCamera : AInteractable
{

    [SerializeField] Camera cam;
    bool camOn;
    private void Awake()
    {
        cam.enabled = false;
    }

    public override void Interact()
    {
        if (!camOn) // cam is not currently active
        {
            CameraManager.i.SetActiveCamera(cam);
            camOn = true;

        }
        else
        {
            CameraManager.i.RestorePlayerCam();
            cam.enabled = false;
            camOn = false;
        }
    }
}
