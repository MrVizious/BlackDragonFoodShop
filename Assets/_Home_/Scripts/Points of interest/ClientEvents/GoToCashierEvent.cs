using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using Pathfinding;
using Cysharp.Threading.Tasks;

public class GoToCashierEvent : ClientEvent
{
    public override async UniTask Execute()
    {
        await base.Execute();
        Transform newTarget = FindObjectOfType<Cashier>().GetFreeSpot(client);
        if (newTarget == null)
        {
            LevelManager.Instance.unsatisfiedClients++;
            client.DropItems();
            client.onClientUnsatisfied.Invoke();
            return;
        }
        destinationSetter.target = newTarget;
        await UniTask.WaitUntil(HasArrivedToDestination).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
        Debug.Log("Starting to wait");
        await UniTask.Delay(10000).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
        Debug.Log("Waited for too long!", gameObject);
        LevelManager.Instance.unsatisfiedClients++;
        client.DropItems();
        FindObjectOfType<Cashier>().CustomerLeft(client);
        client.LeaveStore();
        await UniTask.Delay(300).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
        End();
    }
}
