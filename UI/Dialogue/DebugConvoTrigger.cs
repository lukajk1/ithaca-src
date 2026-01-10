using UnityEngine;

public class DebugConvoTrigger : MonoBehaviour
{
    [SerializeField] PixelCrushers.DialogueSystem.Wrappers.DialogueSystemTrigger trigger;
    [SerializeField] bool isEnabled;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K) && isEnabled)
        {
            trigger.OnUse();
        }
    }
}
