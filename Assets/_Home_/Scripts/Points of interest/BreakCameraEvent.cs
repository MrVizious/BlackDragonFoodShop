using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using TypeReferences;
using Cysharp.Threading.Tasks;

public class BreakCameraEvent : ClientEvent
{
    public override void Setup(EventQueue newQueue)
    {
        base.Setup(newQueue);
    }

    public override async UniTask Execute()
    {
        await base.Execute();
        await UniTask.Delay((int)durationInSeconds * 1000 / 2);
        pointOfInterest.GetComponent<SecurityCamera>().SetWorking(false);
        await UniTask.Delay((int)durationInSeconds * 1000 / 2);
        End();
    }

}
