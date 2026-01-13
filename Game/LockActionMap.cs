using System.Collections.Generic;
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

    private List<object> lockList = new();

    public void ModifyLockList(bool isAdding, object obj)
    {

        if (isAdding)
        {
            if (!lockList.Contains(obj)) lockList.Add(obj);
        }
        else lockList.Remove(obj);

        if (lockList.Count > 0)
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
