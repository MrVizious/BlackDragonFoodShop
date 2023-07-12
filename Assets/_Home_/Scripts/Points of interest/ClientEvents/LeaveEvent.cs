using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using Pathfinding;
using Cysharp.Threading.Tasks;

public class LeaveEvent : ClientEvent
{
    private AIPath aiPath;
    private AIDestinationSetter _destinationSetter;
    public AIDestinationSetter destinationSetter
    {
        get
        {
            if (_destinationSetter == null) _destinationSetter = GetComponent<AIDestinationSetter>();
            return _destinationSetter;
        }
    }
    public override void Setup(EventQueue newQueue)
    {
        base.Setup(newQueue);
        aiPath = GetComponent<AIPath>();
    }
    public override async UniTask Execute()
    {
        await base.Execute();
        Transform newTarget = GameObject.Find("Exit").transform;
        Debug.Log("New Target is: " + newTarget);
        Debug.Log("AI Destination setter: " + destinationSetter);
        destinationSetter.target = newTarget;
        await UniTask.Delay(200);
        await UniTask.WaitUntil(HasArrivedToDestination);
        await UniTask.Delay(500);
        End();
    }

    private bool HasArrivedToDestination()
    {
        bool hasArrived = aiPath.reachedDestination;
        return hasArrived;
    }

    public override void End()
    {
        Destroy(client.gameObject);
        base.End();
    }

}