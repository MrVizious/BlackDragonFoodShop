using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using TypeReferences;
using Cysharp.Threading.Tasks;

public class BreakCameraEvent : ClientEvent
{
    public override async UniTask Execute()
    {
        await base.Execute();
        await UniTask.Delay((int)durationInSeconds * 1000 / 2).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
        pointOfInterest.GetComponent<SecurityCamera>().SetWorking(false);
        pointOfInterest.isActive = false;
        await UniTask.Delay((int)durationInSeconds * 1000 / 2).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
        End();
    }
}
