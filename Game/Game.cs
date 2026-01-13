using NUnit.Framework;
using StarterAssets;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Game : MonoBehaviour
{
    public enum MenuContext
    {
        None,
        Inventory,
        Shop,
        BuyBack,
    }

    public static MenuContext context = MenuContext.None;

    private static bool _isPaused;
    public static bool IsPaused 
    { 
        get
        {
            return _isPaused;
        }
        private set
        {
            if (value != _isPaused)
            {
                _isPaused = value;
                PauseUpdated?.Invoke(value);
                //Debug.Log("pause updated");
                if (value)
                {
                    Time.timeScale = 0;
                }
                else
                {
                    Time.timeScale = 1f;
                }
            }
        }
    }
    private static bool isInDialogue;
    public static bool IsInDialogue { get; set; }
    public Transform PlayerTransform;
    public Camera PlayerCamera;

    public static Action<bool> PauseUpdated;
    public static Game Instance { get; private set; }

    private static CursorLockMode _cursorLockState;
    public static CursorLockMode CursorLockState
    {
        get => _cursorLockState;
        private set
        {
            if (_cursorLockState != value)
            {
                _cursorLockState = value;
                Cursor.lockState = value;
            }
        }
    }

    private static List<object> cursorLockList = new();
    public static void ModifyCursorUnlockList(bool isAdding, object obj)
    {
        if (isAdding)
        {
            if (cursorLockList.Contains(obj)) return; // no need to modify then
            else
            {
                cursorLockList.Add(obj);
            }
        }
        else cursorLockList.Remove(obj);

        if (cursorLockList.Count > 0)
        {
            CursorLockState = CursorLockMode.None;
            CustomStarterAssetsInputs.i.cursorInputForLook = false;
        }
        else
        {
            CursorLockState = CursorLockMode.Locked;
            CustomStarterAssetsInputs.i.cursorInputForLook = true;
        }
    }

    private static List<object> pauseList = new();
    public static void ModifyPauseList(bool isAdding, object obj)
    {
        if (isAdding && !pauseList.Contains(obj)) pauseList.Add(obj);
        else pauseList.Remove(obj);

        IsPaused = pauseList.Count > 0;
    }

    public static float mouseSensitivity = 250f;

    public static bool talkingToNPC;

    private void Awake()
    {
        if (Instance != null) 
        {
            Debug.LogError($"More than one instance of {Instance} in scene");
        }

        Instance = this;

        Game.CursorLockState = CursorLockMode.Locked;
    }
}
