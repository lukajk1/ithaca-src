using UnityEngine;
using PixelCrushers.DialogueSystem;

public class GlobalDialogueRefs : MonoBehaviour
{
    public static PixelCrushers.DialogueSystem.Wrappers.DialogueSystemTrigger trigger;
    [SerializeField] PixelCrushers.DialogueSystem.Wrappers.DialogueSystemTrigger _trigger;

    public static DialogueDatabase db;
    [SerializeField] DialogueDatabase _db;

    [SerializeField] [ConversationPopup(true, true)] string _sleepingConversationName;

    public static GlobalDialogueRefs i;
    private void Awake()
    {
        i = this;

        trigger = _trigger;
        db = _db;
    }

    public void CharacterIsSleepingAlert()
    {
        trigger.conversation = i._sleepingConversationName;
        trigger.OnUse();
    }
}