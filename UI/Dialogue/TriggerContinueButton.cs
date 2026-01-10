using PixelCrushers.DialogueSystem;
using UnityEngine;
using UnityEngine.UI;

public class TriggerContinueButton : MonoBehaviour
{
    private Button button;
    private const float delay = 0.24f;

    private float elapsedSinceLastPress;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    // Update is called once per frame
    void Update()
    {
        elapsedSinceLastPress += Time.unscaledDeltaTime;

        if (Input.GetKeyDown(KeyCode.Mouse0) && elapsedSinceLastPress > delay)
        { 
            // the button is configured via inspector to call the text animator fastforward method, as the guide pixelcrushers made configures the basic fastforward method that way. I'll just leave it like that instead of triggering by code
            button.onClick.Invoke();
            elapsedSinceLastPress = 0;
        }

    }
}
