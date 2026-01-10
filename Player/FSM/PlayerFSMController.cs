using Animancer;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerFSMController : MonoBehaviour
{
    private AState currentState;

    [SerializeField] public AState idleState;
    [SerializeField] public AState walkState;
    [SerializeField] public AState jumpState;
    [SerializeField] public AState fishingState;

    [HideInInspector] public bool isGrounded => thirdPersonController.Grounded;

    [SerializeField] InputActionReference jump;
    [SerializeField] CustomThirdPersonController thirdPersonController;
    void Start()
    {
        MoveToState(idleState);
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

    public void MoveToState(AState state)
    {
        currentState?.Exit();
        currentState = state;
        state?.Enter();
    }

    void Update()
    {
        currentState.Tick();
        
    }

    private void FixedUpdate()
    {

    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            MoveToState(jumpState);
        }
    }
}
