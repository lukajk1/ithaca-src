using Animancer;
using UnityEngine;
using UnityEngine.InputSystem;

public class SFishing : AState, IState
{
    [SerializeField] private ClipTransition castDrawBack;
    [SerializeField] private ClipTransition castThrow;
    [SerializeField] private ClipTransition castIdle;
    [SerializeField] private ClipTransition fishingIdle;

    [SerializeField] private PlayerCast cast;
    [SerializeField] private AudioClip castSwoosh;
    [SerializeField] private GameObject castSwooshVFX;

    private const float SWOOSH_VFX_DURATION = 0.12f;

    private AnimancerState castDrawBackState;
    private AnimancerState throwState;
    private bool hasThrown;
    public override void Enter()
    {
        hasThrown = false;
        castDrawBackState = animancer.Play(castDrawBack);

        if (castDrawBackState.Events(this, out AnimancerEvent.Sequence events))
        {
            events.OnEnd = CastBackIdle;
        }
    }
    private void CastBackIdle()
    {
        if (!hasThrown) animancer.Play(castIdle);
    }
    private void EventThrow()
    {
        cast.ApplyChargedForce();
    }
    private void ThrowStart()
    {
        SoundManager.Play(new SoundData(castSwoosh));

    }
    private void ThrowEnd()
    {
        animancer.Play(fishingIdle);
    }
    private void ThrowVFX()
    {
        castSwooshVFX.SetActive(true);

        LeanTween.delayedCall(castSwooshVFX, SWOOSH_VFX_DURATION, () => {
            castSwooshVFX.SetActive(false);
        });
    }

    public void Throw()
    {
        throwState = animancer.Play(castThrow);
        hasThrown = true;

        if (throwState.Events(this, out AnimancerEvent.Sequence events))
        {
            events.Add(0.176f, ThrowStart);
            events.Add(0.25f, ThrowVFX);
            events.Add(0.441f, EventThrow);
            events.OnEnd = ThrowEnd;
        }
    }
    public override void Exit()
    {

    }
    public override void Tick()
    {
        
    }
}
