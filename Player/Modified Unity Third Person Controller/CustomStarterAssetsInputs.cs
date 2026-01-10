using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    public class CustomStarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;
        public bool jump;
        public bool slowWalk;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorInputForLook = true;

        [Header("Input Action References")]
        [SerializeField] private InputActionReference moveAction;
        [SerializeField] private InputActionReference lookAction;
        [SerializeField] private InputActionReference jumpAction;
        [SerializeField] private InputActionReference slowWalkAction;

        [Header("Mouse Look Settings")]
        [SerializeField] private float lookSensitivity = 1f;

        public static CustomStarterAssetsInputs i;
        public void Awake()
        {
            i = this;
        }
        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        public void JumpInput(bool newJumpState)
        {
            jump = newJumpState;
        }

        public void SprintInput(bool newSprintState)
        {
            slowWalk = newSprintState;
        }

        private void OnEnable()
        {
            moveAction.action.Enable();
            lookAction.action.Enable();
            jumpAction.action.Enable();
            slowWalkAction.action.Enable();
        }

        private void OnDisable()
        {
            moveAction.action.Disable();
            lookAction.action.Disable();
            jumpAction.action.Disable();
            slowWalkAction.action.Disable();
        }

        private void Update()
        {
            MoveInput(moveAction.action.ReadValue<Vector2>());

            if (cursorInputForLook)
            {
                Vector2 rawLookInput = lookAction.action.ReadValue<Vector2>();
                LookInput(new Vector2(rawLookInput.x, -rawLookInput.y) * lookSensitivity);
            }

            JumpInput(jumpAction.action.IsPressed());
            SprintInput(slowWalkAction.action.IsPressed());
        }
    }
}
