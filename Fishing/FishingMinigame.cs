using UnityEngine;
using QFSW.QC;

public class FishingMinigame : MonoBehaviour
{
    [SerializeField] private Transform gameParent;

    [SerializeField] private Gradient gradient;
    [SerializeField] private Camera minigameCamera;
    [SerializeField] private GameObject buttonPressPrompt;
    [SerializeField] private GameObject caret;

    private float radiusOfReelCircle = 0.5f;
    private float rotationSpeed = 110f;
    private float minimumSpacingDeg = 35f;
    private float timingWindowDeg = 35f;

    private ConcreteItem caughtFish;

    public static FishingMinigame i;
    private void Awake()
    {
        i = this;

        minigameCamera.gameObject.SetActive(false);
        QuantumRegistry.RegisterObject(this);
    }

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
        GameObject prompt = Instantiate(buttonPressPrompt, transform);
        prompt.transform.localPosition = randomPointOnCircle;

        minigameCamera.gameObject.SetActive(true);
        LockActionMap.i.ModifyLockList(ActionMapType.Main, true, this);
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
