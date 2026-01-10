using Animancer;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class SWalk : AState, IState
{
    [SerializeField] private ClipTransition walk;
    [SerializeField] private InputActionReference moveBinding;
    [SerializeField] private CustomStarterAssetsInputs input;

    private float originalPlaySpeed;
    private float walkSpeed = 0.85f;
    void Awake()
    {
        originalPlaySpeed = walk.Speed;
    }
    public override void Enter()
    {
        animancer.Play(walk);
    }

    public override void Exit()
    {

    }

    public override void Tick()
    {
        Vector2 inputDirection = moveBinding.action.ReadValue<Vector2>();
        if (inputDirection.sqrMagnitude == 0 && controller.isGrounded)
        {
            controller.MoveToState(controller.idleState);
        }

        if (input.slowWalk)
        {
            walk.Speed = walkSpeed;
        }
        else
        {
            walk.Speed = originalPlaySpeed;
        }
    }
}