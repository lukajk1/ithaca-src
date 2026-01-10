using PixelCrushers.DialogueSystem;
using UnityEngine;
using System.Collections.Generic;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private Camera mainCam;
    [SerializeField] private Camera convoCam;
    [SerializeField] private CheckCamView playerCamView;

    [SerializeField] private List<ActorCamAssociation> actorCamAssociations;

    public static CameraManager i;
    private bool inConversation;

    private void Awake()
    {
        i = this;
        convoCam.gameObject.SetActive(false);
    }

    public void SetCamera(Subtitle subtitle)
    {
        if (!inConversation)
        {
            inConversation = true;
            mainCam.gameObject.SetActive(false);
            convoCam.gameObject.SetActive(true);
        }

        if (subtitle.speakerInfo.isPlayer)
        {
            convoCam.transform.position = playerCamView.transform.position;
            convoCam.transform.rotation = playerCamView.transform.rotation;
        }
        else
        {
            foreach (var item in actorCamAssociations)
            {
                if (item.actorName == subtitle.speakerInfo.nameInDatabase)
                {
                    convoCam.transform.position = item.cameraPlacement.position;
                    convoCam.transform.rotation = item.cameraPlacement.rotation;
                    convoCam.gameObject.SetActive(true);
                    return;
                }
            }

            Debug.LogWarning("Missing actor camera entry");
        }
    }

    public void SetCamera(string actorName)
    {
        foreach (var item in actorCamAssociations)
        {
            if (actorName == item.actorName)
            {
                convoCam.transform.position = item.cameraPlacement.position;
                convoCam.transform.rotation = item.cameraPlacement.rotation;
                convoCam.gameObject.SetActive(true);
                return;
            }
        }

        Debug.LogError("Camera DNE for that character!");
    }

    public void RestorePlayerCam()
    {
        convoCam.gameObject.SetActive(false);
        mainCam.gameObject.SetActive(true);
        inConversation = false;
    }

    public void SetActiveCamera(Camera cam)
    {
        cam.enabled = true;
        mainCam.gameObject.SetActive(false);
    }
}


[System.Serializable]

public struct ActorCamAssociation
{
    [ActorPopup] public string actorName;
    public Transform cameraPlacement;
}
