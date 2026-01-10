using PixelCrushers.DialogueSystem;
using UnityEngine;

public class CheckConversationPlayed : MonoBehaviour
{
    public static bool Check(string conversationTitle)
    {
        var conversation = DialogueManager.masterDatabase.GetConversation(conversationTitle);

        if (conversation == null) return false; // (Invalid conversation title.)

        return DialogueLua.GetSimStatus(conversation.id, 0) == DialogueLua.WasDisplayed;
    }
}
