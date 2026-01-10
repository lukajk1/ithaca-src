using Animancer;
using UnityEngine;
using UnityEngine.InputSystem;

public class SIdle : AState, IState
{
    [SerializeField] private ClipTransition idle;
    [SerializeField] private InputActionReference moveBinding;
    public override void Enter()
    {
        animancer.Play(idle);
    }

    public override void Exit()
    {

    }

    public override void Tick()
    {
        Vector2 inputDirection = moveBinding.action.ReadValue<Vector2>();

        if (inputDirection.sqrMagnitude > 0 && controller.isGrounded)
        {
            controller.MoveToState(controller.walkState);
        }
    }
}