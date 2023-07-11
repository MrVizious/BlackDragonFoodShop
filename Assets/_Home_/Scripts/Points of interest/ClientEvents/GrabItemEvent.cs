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
            Debug.Log("Item taken!");
        }
        else
        {
            Debug.Log("There are no items!");
        }
        await UniTask.Delay((int)durationInSeconds * 1000 / 2).AttachExternalCancellation(this.GetCancellationTokenOnDestroy());
        End();
    }
}