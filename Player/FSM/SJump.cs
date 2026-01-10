using Animancer;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class SJump : AState, IState
{
    [SerializeField] private ClipTransition jump;
    [SerializeField] private CustomThirdPersonController tpController;
    [SerializeField] private float delay = 0.3f; // tweak in editor
    private bool canExit = false;
    public override void Enter()
    {
        animancer.Play(jump);

        LeanTween.delayedCall(delay, () => {
            canExit = true;
        });
    }

    public override void Exit()
    {
        canExit = false;
    }

    public override void Tick()
    {
        if (tpController.Grounded && canExit) controller.MoveToState(controller.idleState);
    }
}