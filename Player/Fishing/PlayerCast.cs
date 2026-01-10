using GogoGaga.OptimizedRopesAndCables;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;

public class PlayerCast : MonoBehaviour
{
    [Header("Main")]
    [SerializeField] private Transform castOrigin;
    [SerializeField] private GameObject fishingLine;
    [SerializeField] private RodManager rodManager;
    [SerializeField] private Rope rope;
    [SerializeField] private BobberManager bobber;

    [Header("Player")]
    [SerializeField] private InputActionReference chargeAction;
    [SerializeField] private PlayerFSMController playerFSM;
    private SFishing fishingState;

    [Header("Force Settings")]
    [SerializeField] private float maxForce = 10f;
    [SerializeField] private float maxChargeTime = 1.5f;
    [SerializeField] private AnimationCurve forceCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    [Header("Direction")]
    [SerializeField] private Transform forceDirection; // Optional: use this transform's forward
    [SerializeField] private Vector3 defaultDirection = Vector3.forward;
    [SerializeField] private ForceMode forceMode = ForceMode.Impulse;
    [Range(0f, 90f)]
    [SerializeField] private float launchAngle = 45f; // Angle in degrees from horizontal

    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;

    [Header("Audio")]
    [SerializeField] private AudioClip castBegin;

    private float _chargeTime = 0f;
    private bool _isCharging = false;
    private float _chargePercent = 0f;

    private void OnEnable()
    {
        if (chargeAction != null)
        {
            chargeAction.action.Enable();
        }
    }

    private void OnDisable()
    {
        if (chargeAction != null)
        {
            chargeAction.action.Disable();
        }
    }

    private void OnChargeStart()
    {
        //rodManager.SetLineAndBobber(false);
        SoundManager.Play(new SoundData(castBegin));
        playerFSM.MoveToState(playerFSM.fishingState);
        fishingState = playerFSM.fishingState as SFishing;

        CastUI.i.StartCast(1.2f);
    }

    private void OnCharging(float percent)
    {
        // Update UI bar, play charging sound, etc.
    }

    private void OnChargeRelease(float force, float percent)
    {
        fishingState.Throw();
        //rodManager.SetLineAndBobber(true);
    }
    private void Update()
    {
        if (chargeAction == null || !rodManager.rodEquipped) return;

        // Check if button is being held
        bool isPressed = chargeAction.action.IsPressed();

        if (isPressed)
        {
            if (!_isCharging)
            {
                // if there is no space in the inventory, don't start charge. This should trigger a notice at some point
                if (!InventoryManager.i.HasFreeSlot()) return;
                if (Game.context != Game.MenuContext.None) return;

                // Just started charging
                _isCharging = true;
                _chargeTime = 0f;
                OnChargeStart();
            }

            // Accumulate charge time (capped at maxChargeTime)
            //_chargeTime = Mathf.Min(_chargeTime + Time.deltaTime, maxChargeTime);
            //_chargePercent = _chargeTime / maxChargeTime;

            OnCharging(_chargePercent);
        }
        else if (_isCharging)
        {
            // --- FIX IS HERE ---
            // Button released - apply force/start throw animation
            _isCharging = false;

            // 1. Get final charge value from UI slider
            _chargePercent = CastUI.i.sliderValue;

            // 2. Stop the UI charge animation immediately
            CastUI.i.StopCast();

            // 3. Trigger the animation throw sequence (this is where the throw animation starts)
            fishingState.Throw();
        }
    }

    public void ApplyChargedForce()
    {
        bobber.ClearPhysics();
        bobber.MoveToPos(castOrigin.position);
        bobber.Reset();

        // Calculate force based on charge time using curve
        float curveValue = forceCurve.Evaluate(_chargePercent);
        float force = maxForce * curveValue;

        // Determine base horizontal direction
        Vector3 horizontalDirection = forceDirection != null
            ? forceDirection.forward
            : defaultDirection;

        // Flatten to horizontal plane (remove Y component)
        horizontalDirection.y = 0;
        horizontalDirection.Normalize();

        // Calculate direction at specified angle
        float angleRad = launchAngle * Mathf.Deg2Rad;
        Vector3 direction = (horizontalDirection * Mathf.Cos(angleRad) + Vector3.up * Mathf.Sin(angleRad)).normalized;

        // Apply force
        bobber.Cast(direction * force, forceMode);

        if (showDebugInfo)
        {
            Debug.Log($"Applied force: {force:F0} ({_chargePercent:P0} charge) at {launchAngle}° angle");
        }

        OnChargeRelease(force, _chargePercent);

        // Reset
        _chargeTime = 0f;
        _chargePercent = 0f;
    }

    // Optional callbacks for visual/audio feedback

    // Public getters for UI/feedback systems
    public float GetChargePercent() => _chargePercent;
    public bool IsCharging() => _isCharging;
    public float GetCurrentForce() => maxForce * forceCurve.Evaluate(_chargePercent);
}