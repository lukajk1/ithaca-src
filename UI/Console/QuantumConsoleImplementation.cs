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

    [Command("int-prop")]
    protected static int someInt(int myInt) { 
        return myInt + 5; 
    }
}
