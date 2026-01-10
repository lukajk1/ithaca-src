using PixelCrushers.DialogueSystem;
using UnityEngine;
using TMPro;

public class DialogueWithBuiltInTypewriter : MonoBehaviour
{
    [SerializeField] private DialogueSystemTrigger trigger;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private UnityUITypewriterEffect typewriterEffect;
    
    void Start()
    {
        // Add typewriter component if not already present
        if (typewriterEffect == null)
        {
            typewriterEffect = dialogueText.gameObject.AddComponent<UnityUITypewriterEffect>();
        }
        
        // Configure typewriter settings
        typewriterEffect.charactersPerSecond = 20f;
        typewriterEffect.playOnEnable = false;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            trigger.OnUse();
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            HandleContinue();
        }
    }
    
    public void OnSubtitle(Subtitle subtitle)
    {
        string text = subtitle.formattedText.text;
        
        // DON'T set the text manually - let typewriter handle it
        // dialogueText.text = text; // Remove this line
        
        // Start typewriter with the text
        typewriterEffect.StartTyping(text);
    }
    
    private void HandleContinue()
    {
        if (PixelCrushers.DialogueSystem.DialogueManager.isConversationActive)
        {
            if (typewriterEffect.isPlaying)
            {
                // Complete typewriter immediately
                typewriterEffect.Stop();
            }
            else
            {
                // Continue to next line
                PixelCrushers.DialogueSystem.DialogueManager.standardDialogueUI.OnContinue();
            }
        }
    }
}