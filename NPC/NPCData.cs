using PixelCrushers.DialogueSystem;
using UnityEngine;

public static class NPCConstants
{
    public const float NPCMenuFadeLength = 0.65f;
}

[System.Serializable]
public struct NPCConversation
{
    [VariablePopup] [SerializeField] public string readVariable;
    [TextArea(2, 4)] [SerializeField] public string itemTextForConversation;

    [SerializeField] [ConversationPopup(true, true)]
    public string conversation;
}

[System.Serializable]
public struct NPCQuest
{
    [SerializeField] [ConversationPopup(true, true)]
    public string quest;
}
