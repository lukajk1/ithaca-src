using QFSW.QC;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class QuantumConsoleImplementation : MonoBehaviour
{
    #region setup
    [SerializeField] QuantumConsole _qc;
    void Start()
    {
        _qc = _qc
            ?? GetComponent<QuantumConsole>()
            ?? QuantumConsole.Instance;

        if (_qc)
        {
            _qc.OnActivate += OnActivate;
            _qc.OnDeactivate += OnDeactivate;
        }

    }

    void OnActivate()
    {
        //Debug.Log("QC Activated");
        Game.ModifyPauseList(true, this);
        LockActionMap.i.Lock(true);

    }

    void OnDeactivate()
    {
        //Debug.Log("QC Deactivated");
        LockActionMap.i.Lock(false);
        Game.ModifyPauseList(false, this);

    }
    #endregion

    [Command("time-set")]
    protected static void setTime(int hours, int seconds = 0) {
        GTime.SetTime(hours, seconds);
    }

    [Command("do-time-tick")]
    protected static void doTimeTick(bool boolean) {
        GTime.doTimeTick = boolean;
    }
}
