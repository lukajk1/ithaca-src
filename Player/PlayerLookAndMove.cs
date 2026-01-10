using System.Collections;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerLookAndMove : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Camera mainCamera; 

    [SerializeField] private Rigidbody rb; 

    [SerializeField] private Transform lastJumpedFrom;

    [SerializeField] private GroundedHitbox groundedHitbox;
    [SerializeField] private InputActionReference slowWalk;
    private const float slowWalkSpeedRatio = 0.48f;

    public static bool CamLocked;

    private float moveSpeed = 6f;
    public float MoveSpeed
    {
        get
        {
            return moveSpeed;
        }
        set
        {
            if (value > 0) 
            {
                moveSpeed = value;
            }
        }
    }

    private float jumpForce = 480f;
    public float JumpForce
    {
        get
        {
            return jumpForce;
        }
        set
        {
            if (value > 0)
            {
                jumpForce = value;
            }
        }
    }
    private float initJumpForce;
    private float initMoveSpeed;

    //private float sensitivity = 250f;
    private float xRotation;
    private float yRotation;

    [HideInInspector] public bool isGrounded;

    //private Game game;

    private float positionSaveInterval = 5f;
    private float timeSinceLastPositionSave = 0f;

    private float jumpCDTimer = 0.7f; // to prevent "bouncing" up off ramps
    //private bool CDElapsedSinceLastJump = true;
    private Coroutine jumpedWithinTheWindowCRTimer;
    //private float lastPhysicsUpdateYPos = 0f;
    //bool groundedLastPhysicsUpdate;

    //private bool isClearOfGrounded;
    //private bool hasTouchedDown;


    [SerializeField] InputActionReference move;
    [SerializeField] InputActionReference jump;

    public static event Action OnLandingFromJump;

    private void Awake()
    {
        initJumpForce = JumpForce;
        initMoveSpeed = MoveSpeed;

        rb = player.GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotation;

        lastJumpedFrom.position = rb.position;
    }

    private void OnEnable()
    {
        jump.action.Enable();
        jump.action.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        jump.action.performed -= OnJumpPerformed;
        jump.action.Disable();
    }
    private void FixedUpdate()
    {
        timeSinceLastPositionSave += Time.fixedDeltaTime;

        bool wasNotGrounded = !isGrounded;
        isGrounded = groundedHitbox.IsGrounded();

        if (wasNotGrounded && isGrounded)
        {
            OnLandingFromJump?.Invoke();            
        }

        if (timeSinceLastPositionSave > positionSaveInterval
            && Physics.Raycast(transform.position, Vector3.down, out RaycastHit hit, 0.35f)
            && hit.collider.TryGetComponent<ObjectMaterial>(out var mat)
            && mat.type != ObjectMaterial.Type.Water)
        {
            lastJumpedFrom.position = rb.position;
            timeSinceLastPositionSave = 0f;
        }

        //float currentYPos = rb.gameObject.transform.position.y;

        //if (lastPhysicsUpdateYPos < currentYPos)
        //{
        //    if (jumpedWithinTheWindowCRTimer == null && groundedLastPhysicsUpdate)
        //    {
        //        rb.MovePosition(new Vector3(rb.gameObject.transform.position.x, lastPhysicsUpdateYPos, rb.gameObject.transform.position.z));
        //    }
        //}

        //lastPhysicsUpdateYPos = currentYPos;
    }

    private void Update()
    {
        if (!Game.IsPaused && !Game.IsInDialogue)
        {
            Vector3 movement = DetermineMovementVector();
            rb.linearVelocity = new Vector3(movement.x, rb.linearVelocity.y, movement.z);

            DetermineCamMovement();
        }
    }

    public void ResetToJumpSavePos()
    {
        rb.MovePosition(lastJumpedFrom.position);
    }
    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (isGrounded && !Game.IsPaused) // possibly do a check to Mathf.Abs(rb.linearVelocity.y) < 0.01f but this could return true in cases where it shouldn't
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z); // Reset vertical velocity
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);


            if (jumpedWithinTheWindowCRTimer != null)
            {
                StopCoroutine(jumpedWithinTheWindowCRTimer);
            }
            jumpedWithinTheWindowCRTimer = StartCoroutine(DelayedCR());

        }
    }

    private IEnumerator DelayedCR()
    {
        yield return new WaitForSeconds(jumpCDTimer);
        jumpedWithinTheWindowCRTimer = null;
    }

    private void DetermineCamMovement()
    {
        if (CamLocked) return;

        xRotation -= Input.GetAxis("Mouse Y") * Time.deltaTime * Game.mouseSensitivity;
        yRotation += Input.GetAxis("Mouse X") * Time.deltaTime * Game.mouseSensitivity;

        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        mainCamera.transform.localEulerAngles = new Vector3(xRotation, yRotation, 0);
        transform.localEulerAngles = new Vector3(0, yRotation, 0); // Only apply yaw to player
    }
    private Vector3 DetermineMovementVector()
    {
        float calculatedMS = MoveSpeed;
        if (slowWalk.action.IsPressed())
        {
            calculatedMS = calculatedMS * slowWalkSpeedRatio;
        }

        Vector2 moveDir = move.action.ReadValue<Vector2>().normalized * calculatedMS;
        //if (moveDir != Vector2.zero) Debug.Log("reading movement input");

        // Get only the horizontal (yaw) rotation of the player, ignoring the pitch (up/down)
        float yRotation = transform.eulerAngles.y;

        // Calculate movement relative to the player's current horizontal rotation
        Vector3 forward = new Vector3(Mathf.Sin(yRotation * Mathf.Deg2Rad), 0, Mathf.Cos(yRotation * Mathf.Deg2Rad)) * moveDir.y;
        Vector3 right = transform.right * moveDir.x;

        Vector3 combined = forward + right;
        return new Vector3(combined.x, 0, combined.z);
    }


    public void ResetValues()
    {
        JumpForce = initJumpForce;
        MoveSpeed = initMoveSpeed;
    }

}
