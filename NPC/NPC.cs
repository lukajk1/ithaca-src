using NaughtyAttributes;
using PixelCrushers.DialogueSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

public class NPC : AInteractable
{
    [ActorPopup] [SerializeField] private string actorName;
    public override Type interactType => Type.Talk;

    [Header("Introduction")]
    [SerializeField] private bool hasIntroduction;
    [ShowIf("hasIntroduction")] [ConversationPopup(true, true)] [SerializeField]
    public string introConversation;

    [ShowIf("hasIntroduction")] [VariablePopup] [SerializeField] 
    public string introReadVariable;

    [Header("Shop")]
    [SerializeField] private bool hasShop;
    [ShowIf("hasShop")] [SerializeField] 
    private ShopData shopData;

    [TextArea(1, 5)] [SerializeField] List<string> greetings;

    // these don't work.. refactor into struct at some point..
    [SerializeField] (int hours, int minutes) wakeTime;
    [SerializeField] (int hours, int minutes) sleepTime;

    [SerializeField] [Range(0.5f, 1.5f)] float voicePitch = 1f;
    // a slower/faster speech field would be good as well? 

    [HorizontalLine(color: EColor.Blue)]
    [SerializeField] List<NPCConversation> conversations;

    [HorizontalLine(color: EColor.Blue)]
    [SerializeField] List<NPCQuest> quests;

    public override void Interact()
    {
        if (hasIntroduction && !DialogueLua.GetVariable(introReadVariable).AsBool)
        {
            NPCMenu.i.trigger.conversation = introConversation;
            Action startIntro = () =>
            {
                Game.ModifyCursorUnlockList(true, NPCMenu.i); // enable the cursor for intro
                NPCMenu.i.trigger.OnUse();
            };
            ScreenFade.i.Fade(NPCConstants.NPCMenuFadeLength, LeanTweenType.linear, LeanTweenType.linear, startIntro);

            // set to delegate field
            MyCustomConversationEventBroadcasts.i.openMenuOnConversationEndCallback = () =>
            {
                NPCMenu.i.Open(actorName, greetings, voicePitch, conversations, quests, shopData);
            };
            return;
        }

        if (IsAwake())
        {
            Action openMenu = () => NPCMenu.i.Open(actorName, greetings, voicePitch, conversations, quests, shopData);
            ScreenFade.i.Fade(NPCConstants.NPCMenuFadeLength, LeanTweenType.linear, LeanTweenType.linear, openMenu);
        }
        else
        {
            GlobalDialogueRefs.i.CharacterIsSleepingAlert();
        }
    }

    private bool IsAwake()
    {
        //int currentHours = GTime.Time.hours;
        //int currentMinutes = GTime.Time.minutes;

        //int wakeTimeInMinutes = wakeTime.hours * 60 + wakeTime.minutes;
        //int sleepTimeInMinutes = sleepTime.hours * 60 + sleepTime.minutes;
        //int currentTimeInMinutes = currentHours * 60 + currentMinutes;

        //return currentTimeInMinutes >= wakeTimeInMinutes && currentTimeInMinutes < sleepTimeInMinutes;
        return true;
    }


}

