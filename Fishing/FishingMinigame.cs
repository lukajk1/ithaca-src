using UnityEngine;
using UnityEngine.InputSystem;
using QFSW.QC;
using UnityEngine.UI;

public class FishingMinigame : MonoBehaviour
{
    [SerializeField] private Transform gameParent;
    [SerializeField] private bool debugStatements;
    [Space(15)]

    [SerializeField] private Gradient gradient;
    [SerializeField] private Camera minigameCamera;
    [SerializeField] private GameObject buttonPressPrompt;
    [SerializeField] private GameObject caret;
    [SerializeField] private SpriteRenderer reelWheel;
    [SerializeField] private Slider progressSlider;

    [SerializeField] private InputActionReference reelAction;

    [SerializeField] private AudioClip reelSound;

    private float radiusOfReelCircle = 0.70f;
    private float rotationSpeed = 110f;
    private float minimumSpacingDeg = 35f;
    private float timingWindowDeg = 35f;
    private float maximumRotationSpeed = 600f;

    private ConcreteItem caughtFish;
    private GameObject currentPrompt;

    public static FishingMinigame i;

    #region setup
    private void Awake()
    {
        i = this;

        minigameCamera.gameObject.SetActive(false);
        QuantumRegistry.RegisterObject(this);
    }

    private void OnEnable()
    {
        if (reelAction != null)
        {
            reelAction.action.Enable();
            reelAction.action.performed += OnReelPressed;
        }
    }

    private void OnDisable()
    {
        if (reelAction != null)
        {
            reelAction.action.performed -= OnReelPressed;
            reelAction.action.Disable();
        }
    }
    #endregion
    private void Update()
    {
        if (minigameCamera.gameObject.activeSelf)
        {
            caret.transform.Rotate(0, 0, rotationSpeed * Time.deltaTime);
        }
    }
    public void Begin(ConcreteItem fish)
    {
        caughtFish = fish;

        // Get random point on unit circle
        float randomAngle = Random.Range(0f, 360f);
        float angleInRadians = randomAngle * Mathf.Deg2Rad;

        Vector2 randomPointOnCircle = new Vector2(
            Mathf.Cos(angleInRadians),
            Mathf.Sin(angleInRadians)
        ) * radiusOfReelCircle;

        // Instantiate button prompt at random position
        currentPrompt = Instantiate(buttonPressPrompt, gameParent);
        currentPrompt.transform.localPosition = randomPointOnCircle;

        reelWheel.color = ColorLookup.LookupRarity(fish.data.rarity);

        LockActionMap.i.ModifyLockList(ActionMapType.Main, true, this);

        minigameCamera.gameObject.SetActive(true);

    }

    private void OnReelPressed(InputAction.CallbackContext context)
    {

        if (!minigameCamera.gameObject.activeSelf || currentPrompt == null) return;

        // caret angle
        float caretAngle = 360f - caret.transform.localEulerAngles.z;

        if (debugStatements) Debug.Log($"caret angle: {caretAngle}");

        // prompt angle
        Vector2 promptPos = currentPrompt.transform.localPosition;
        float promptAngle = Mathf.Atan2(promptPos.x, promptPos.y) * Mathf.Rad2Deg;
        if (promptAngle < 0) promptAngle += 360f;

        if (debugStatements) Debug.Log($"prompt angle: {promptAngle}");


        // Calculate angular difference (accounting for wrap-around)
        float angularDiff = Mathf.DeltaAngle(caretAngle, promptAngle);

        // Check if within timing window
        if (Mathf.Abs(angularDiff) <= timingWindowDeg)
        {
            SoundManager.Play(new SoundData(reelSound));
            // Success! Move prompt to new location
            MovePromptToNewLocation();

            // Increase rotation speed by 15%, clamped to maximum
            rotationSpeed = Mathf.Min(rotationSpeed * 1.15f, maximumRotationSpeed);

            //Debug.Log($"Hit! Angular diff: {angularDiff:F1}° - New speed: {rotationSpeed:F1}°/s");
        }
        else
        {
            //Debug.Log($"Miss! Angular diff: {angularDiff:F1}°");
        }
    }

    private void MovePromptToNewLocation()
    {
        // Get random point on unit circle
        float randomAngle = Random.Range(0f, 360f);
        float angleInRadians = randomAngle * Mathf.Deg2Rad;

        Vector3 randomPointOnCircle = new Vector3(
            Mathf.Cos(angleInRadians) * radiusOfReelCircle,
            Mathf.Sin(angleInRadians) * radiusOfReelCircle,
            currentPrompt.transform.localPosition.z
        );

        currentPrompt.transform.localPosition = randomPointOnCircle;
    }


    [Command("end-minigame")]
    public void Close()
    {
        minigameCamera.gameObject.SetActive(false);
        LockActionMap.i.ModifyLockList(ActionMapType.Main, false, this);
        FishingManager.i.OnComplete(caughtFish);
    }
    void MyMethod()
    {
        Color lineColor = gradient.Evaluate(0.5f);
    }
}
