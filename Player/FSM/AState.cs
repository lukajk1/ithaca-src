using Animancer;
using UnityEngine;

public abstract class AState : MonoBehaviour
{
    [SerializeField] protected PlayerFSMController controller;
    [SerializeField] protected AnimancerComponent animancer;
    public abstract void Enter();
    public abstract void Tick();
    public abstract void Exit();
}
