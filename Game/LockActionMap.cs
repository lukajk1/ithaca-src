using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum ActionMapType
{
    Main = 5,
    Minigame = 10,
    Debug = 15,
}

public class LockActionMap : MonoBehaviour
{
    public static LockActionMap i;

    [SerializeField] InputActionAsset inputActions;
    private InputActionMap mainMap;
    private InputActionMap minigameMap;

    [SerializeField] InputActionReference move;

    private void Awake()
    {
        i = this;
    }
    private void Start()
    {
        mainMap = inputActions.FindActionMap("Main");
        minigameMap = inputActions.FindActionMap("Minigame");
    }

    private List<object> lockList = new();

    public void ModifyLockList(ActionMapType type, bool isAdding, object obj)
    {
        InputActionMap targetActionMap = null;

        switch(type)
        {
            case ActionMapType.Main:
                targetActionMap = mainMap;
                break;
            case ActionMapType.Minigame:
                targetActionMap = minigameMap;
                break;
            default:
                targetActionMap = mainMap;
                break;
        }

        if (isAdding)
        {
            if (!lockList.Contains(obj)) lockList.Add(obj);
        }
        else lockList.Remove(obj);

        if (lockList.Count > 0)
        {
            targetActionMap.Disable();
        }
        else
        {
            targetActionMap.Enable();
        }
    }

    public void LockMovement(bool value)
    {
        if (value) move.action.Disable(); 
        else move.action.Enable(); 
    }
}
