using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using Cysharp.Threading.Tasks;

public class StealItemEvent : ClientEvent
{
    private Shelf itemShelf
    {
        get => (Shelf)pointOfInterest;
    }

    public override async UniTask Execute()
    {
        await base.Execute();
        await UniTask.Delay((int)durationInSeconds * 1000 / 2).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
        if ((itemShelf).currentItemCount > 0)
        {
            itemShelf.currentItemCount--;
            client.currentNumberOfItems++;
            if (client.isSeen)
            {
                client.seenStealing = true;
            }
        }
        await UniTask.Delay((int)durationInSeconds * 1000 / 2).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
        End();
    }
}