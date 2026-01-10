using UnityEngine;
using UnityEngine.InputSystem;

public class LockActionMap : MonoBehaviour
{
    public static LockActionMap i;

    [SerializeField] InputActionAsset inputActions;
    private InputActionMap mainMap;

    [SerializeField] InputActionReference move;

    private void Awake()
    {
        i = this;
    }
    private void Start()
    {
        mainMap = inputActions.FindActionMap("Main");
    }

    public void Lock(bool value)
    {
        if (value)
        {
            mainMap.Disable();
        }
        else
        {
            mainMap.Enable();
        }
    }

    public void LockMovement(bool value)
    {
        if (value) move.action.Disable(); 
        else move.action.Enable(); 
    }
}
