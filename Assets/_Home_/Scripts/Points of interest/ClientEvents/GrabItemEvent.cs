using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using Cysharp.Threading.Tasks;

public class GrabItemEvent : ClientEvent
{
    private ItemShelf itemShelf
    {
        get => (ItemShelf)pointOfInterest;
    }

    public override async UniTask Execute()
    {
        await base.Execute();
        await UniTask.Delay((int)durationInSeconds * 1000 / 2).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
        if ((itemShelf).currentItemCount > 0)
        {
            itemShelf.currentItemCount--;
            client.currentNumberOfItems++;
        }
        else
        {
            LevelManager.Instance.unsatisfiedClients++;
            client.DropItems();
            client.LeaveStore();
        }
        await UniTask.Delay((int)durationInSeconds * 1000 / 2).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
        End();
    }
}