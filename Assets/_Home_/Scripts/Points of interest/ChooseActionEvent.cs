using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;

public class ChooseActionEvent : QueueableEvent
{
    public float secondsToLook = 3f;
    public override void Setup(EventQueue newQueue)
    {
        base.Setup(newQueue);
    }
    public override void Execute()
    {
        base.Execute();
        StartCoroutine(LookForSeconds(secondsToLook));
    }

    private IEnumerator LookForSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        End();
    }
}
