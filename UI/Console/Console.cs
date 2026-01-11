using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using UnityEngine.InputSystem;

public class Console : MonoBehaviour
{
    [SerializeField] private GameObject consoleCanvas;
    [SerializeField] TMP_InputField inputField;

    [SerializeField] InputActionReference openConsoleAction;
    [SerializeField] InputActionReference speedUpTime;

    private string lastCommand;

    private PlayerLookAndMove playerController;
    [SerializeField] private GameObject player;

    public static Console i;
    private void Awake()
    {
        i = this;
    }
    private void OnEnable()
    {
        openConsoleAction.action.Enable();
        openConsoleAction.action.performed += Open;
    }

    private void OnDisable()
    {
        openConsoleAction.action.Disable();
        openConsoleAction.action.performed -= Open;
    }

    void Start()
    {
        playerController = FindFirstObjectByType<PlayerLookAndMove>();

        consoleCanvas.SetActive(false);
        inputField.onEndEdit.AddListener(OnSubmit);
    }
    void Update()
    {
        HandleKeyboardCommands();

        if (speedUpTime.action.IsPressed() || Input.GetKeyDown(KeyCode.R))
        {
            GTime.dayLengthInMinutes = 1.0f;
            GTime.nightLengthInMinutes = 0.4f;
            Time.timeScale = 50f;
        }

        if (speedUpTime.action.WasReleasedThisFrame() || Input.GetKeyUp(KeyCode.R))
        {
            GTime.ResetDayNightLengths();
            Time.timeScale = 1f;
        }
    }


    public bool HandleCommand(string commandArg)
    {
        string formatted = commandArg.Trim().ToLower();
        string[] parsed = SplitCommand(formatted);

        if (parsed.Length == 0)
        {
            Debug.LogWarning("Command cannot be empty.");
            return false;
        }

        else if (parsed[0] == "set" || parsed[0] == "s")
        {
            if (parsed[1] == "weather" || parsed[1] == "w")
            {
                return false;
            }
            else if (parsed[1] == "timescale")
            {
                if (float.TryParse(parsed[2], out float scale))
                {
                    Time.timeScale = scale;
                    return true;
                }
                return false;
            }
        }

        else if (parsed[0] == "dotimetick")
        {
            if (parsed[1] == "t" ||  parsed[1] == "true")
            {
                GTime.doTimeTick = true;
                return true;
            }
            else if (parsed[1] == "f" || parsed[1] == "false")
            {
                GTime.doTimeTick = false;
                return true;
            }
            return false;
        }

        else if (parsed[0] == "settime")
        {
            if (int.TryParse(parsed[1], out int hours))
            {
                GTime.SetTime(hours, 0);
                return true;
            }
            else
            {
                return false;
            }
        }

        else if (parsed[0] == "setrain")
        {
            if (parsed[1] == "t" || parsed[1] == "true")
            {
                WeatherController.i.SetRain(true);
                return true;
            }
            else if (parsed[1] == "f" || parsed[1] == "false")
            {
                WeatherController.i.SetRain(false);
                return true;
            }
            else return false;
        }

        //else if (parsed[0] == "setrain")
        //{
        //    if (parsed[1] == "t" ||  parsed[1] == "true")
        //    {
        //        WeatherController.doTimeTick = true;
        //        return true;
        //    }
        //    else if (parsed[1] == "f" || parsed[1] == "false")
        //    {
        //        GTime.doTimeTick = false;
        //        return true;
        //    }
        //    return false;
        //}

        else if (parsed[0] == "tp")
        {
            Transform loc = GameObject.FindAnyObjectByType<Waypoints>().GetWaypoint(parsed[1]);
            if (loc != null)
            {
                //player.GetComponent<Rigidbody>().MovePosition(loc.position);
                player.transform.position = loc.position;
                return true;
            }
            else return false;
        }

        else if (parsed[0] == "fov")
        {
            if (float.TryParse(parsed[1], out float fov))
            {
                FindAnyObjectByType<FOVAndZoom>().SetFOV(fov);
                return true;
            }
        }

        // otherwise
        Debug.LogWarning($"Unknown command: {formatted}");
        return false;
    }


    private void Open(InputAction.CallbackContext ctx)
    {
        if (!consoleCanvas.activeSelf)
        {
            Game.ModifyPauseList(true, this);
            consoleCanvas.SetActive(true);

            LockActionMap.i.Lock(true);
            inputField.ActivateInputField();
        }
    }

    private void Close()
    {
        inputField.text = ""; // Clear the input field
        consoleCanvas.SetActive(false);

        LockActionMap.i.Lock(false);
        Game.ModifyPauseList(false, this);
    }

    private void HandleKeyboardCommands()
    {
        if (consoleCanvas.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Close();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                inputField.text = lastCommand;
                inputField.caretPosition = inputField.text.Length;
            }
        }
    }

    private void OnSubmit(string command)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (HandleCommand(command))
            {
                lastCommand = command;
            }

            Close();
        }
    }

    private string[] SplitCommand(string command)
    {
        return command.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    }


}