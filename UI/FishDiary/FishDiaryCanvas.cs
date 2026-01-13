using ExternPropertyAttributes;
using QFSW.QC;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FishDiaryCanvas : MonoBehaviour
{
    [InfoBox("commands: show-diary, hide-diary")]
    [Space(15)]
    [SerializeField] private Canvas canvas;
    [SerializeField] private TextMeshProUGUI record;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        canvas.gameObject.SetActive(false);
        closeButton.onClick.AddListener(Close);

    }
    private void Start()
    {
        QuantumRegistry.RegisterObject(this);
    }

    [Command("show-diary")]
    void Open()
    {
        Print();
        LockActionMap.i.ModifyLockList(true, this);
        Game.ModifyCursorUnlockList(true, this);
        Game.ModifyPauseList(true, this);
        canvas.gameObject.SetActive(true);
    }
    [Command("hide-diary")]
    void Close()
    {
        LockActionMap.i.ModifyLockList(false, this);
        Game.ModifyCursorUnlockList(false, this);
        Game.ModifyPauseList(false, this);
        canvas.gameObject.SetActive(false);
    }
    public void Print()
    {
        Dictionary<FishData, FishRecord> allRecords = FishDiary.GetAllRecords();

        if (allRecords.Count == 0)
        {
            record.text = "No fish caught yet.";
            return;
        }

        string output = "";
        foreach (var kvp in allRecords)
        {
            FishData fishData = kvp.Key;
            FishRecord fishRecord = kvp.Value;

            output += $"{fishData.displayName} - {fishRecord.timesCaught} caught - Best: {fishRecord.highestQuality:F2}\n";
        }

        record.text = output;
    }
}
