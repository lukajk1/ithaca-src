using PixelCrushers.DialogueSystem;
using System.Collections.Generic;
using UnityEngine;

public class MyCustomConversationEventBroadcasts : MonoBehaviour
{
    public static MyCustomConversationEventBroadcasts i;
    public System.Action openMenuOnConversationEndCallback;

    private void Awake()
    {
        i = this; // all these singletons might come back to bite me but whatever
    }
    public void OnConversationEnd(Transform actor)
    {
        if (i.openMenuOnConversationEndCallback != null)
        {
            i.openMenuOnConversationEndCallback.Invoke();
            i.openMenuOnConversationEndCallback = null;
        }
        else
        {
            NPCMenu.i.SetVisible(true);
            // this only works if there are already values set to the npcmenu items, which will not be the case in the event of the first interaction being an introduction to a character 
        }

        //ActorCameras.i.RestorePlayerCam();
    }

    public void OnConversationLine(Subtitle subtitle)
    {
        CameraManager.i.SetCamera(subtitle);
    }
}
