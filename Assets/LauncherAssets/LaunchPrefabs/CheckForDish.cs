using System;
using System.Threading.Tasks;
using GameCreator.Runtime.Common;
using GameCreator.Runtime.VisualScripting;
using UnityEngine;

[Serializable]
public class CheckForDish : Instruction
{
    public GameObject Launcher;
    public GameObject Player;
    protected override Task Run(Args args)
    {
         
        // Your code here...
        
        return DefaultResult;
    }
}
