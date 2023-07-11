using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using Pathfinding;
using Cysharp.Threading.Tasks;

public class WalkEvent : ClientEvent
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
        destinationSetter.target = pointOfInterest.transform;
        await UniTask.Delay(200);
        await UniTask.WaitUntil(HasArrivedToDestination);
        End();
    }

    private bool HasArrivedToDestination()
    {
        bool hasArrived = aiPath.reachedDestination;
        return hasArrived;
    }

}
