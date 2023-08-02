using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using Pathfinding;
using Cysharp.Threading.Tasks;
using UtilityMethods;

public class LeaveEvent : ClientEvent
{
    public override void Setup(EventQueue newQueue)
    {
        base.Setup(newQueue);
    }
    public override async UniTask Execute()
    {
        await base.Execute();
        Transform newTarget = GameObject.Find("Door").transform;
        destinationSetter.target = newTarget;
        await UniTask.Delay(200);
        await UniTask.WaitUntil(HasArrivedToDestination);
        await UniTask.Delay(500);
        End();
    }

    public override void End()
    {
        if (client.currentNumberOfItems > 0)
        {
            LevelManager.Instance.stolenItems += client.currentNumberOfItems;
        }
        UtilityMethods.UniTaskMethods.DelayedFunction(() => LevelManager.Instance.SpawnRandomClient(), 1f).Forget();
        Destroy(client.gameObject);
        base.End();
    }

}
