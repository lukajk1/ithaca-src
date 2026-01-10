using System.Collections.Generic;
using UnityEngine;

public class OnStartCommandLoader : MonoBehaviour
{

    [SerializeField] private List<string> startupCommands;
    void Start()
    {
        foreach (string command in startupCommands)
        {
            Console.i.HandleCommand(command);
        }
    }
}
