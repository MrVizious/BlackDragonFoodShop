using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using Pathfinding;
using Cysharp.Threading.Tasks;

public class WalkEvent : ClientEvent
{
    public Transform target;
    public override void Setup(EventQueue newQueue)
    {
        base.Setup(newQueue);
    }
    public override async UniTask Execute()
    {
        await base.Execute();
        if (target == null)
        {
            destinationSetter.target = pointOfInterest.transform;
        }
        else
        {
            destinationSetter.target = target;
        }
        await UniTask.WaitUntil(HasArrivedToDestination).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
        End();
    }


}
