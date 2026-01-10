using PixelCrushers.DialogueSystem;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ConversationMenuItem : MonoBehaviour
{

    string readVariable;
    string linkedConversation;

    [SerializeField] TextMeshProUGUI itemText;

    PixelCrushers.DialogueSystem.Wrappers.DialogueSystemTrigger trigger;
    DialogueDatabase db;

    Button button;

    public void Initialize(NPCConversation conv, PixelCrushers.DialogueSystem.Wrappers.DialogueSystemTrigger trigger, DialogueDatabase db)
    {
        this.readVariable = conv.readVariable;
        this.linkedConversation = conv.conversation;
        itemText.text = conv.itemTextForConversation;
        this.trigger = trigger;
        this.db = db;

        SetDisplayColor();

        button = GetComponent<Button>();
        button.onClick.AddListener(StartConversation);
    }

    void StartConversation()
    {
        //Debug.Log("attempted to start conversation");
        NPCMenu.i.SetVisible(false);
        trigger.conversation = linkedConversation;
        trigger.OnUse();
    }

    void SetDisplayColor()
    {
        Conversation conv = db.GetConversation(linkedConversation);

        string luaVariableName = readVariable;

        if (DialogueLua.DoesVariableExist(luaVariableName))
        {
            bool read = DialogueLua.GetVariable(luaVariableName).AsBool;
            //optionText.color = read ? disabledColor : activeColor;
        }
        else
        {
            Debug.LogError($"DialogueLua variable '{luaVariableName}' does not exist!");
            //optionText.color = disabledColor; 
        }
    }

}
