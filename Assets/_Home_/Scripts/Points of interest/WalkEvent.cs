using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using Pathfinding;

public class WalkEvent : QueueableEvent
{
    private AIPath seeker;
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
        seeker = GetComponent<AIPath>();
    }
    public override void Execute()
    {
        base.Execute();
        StartCoroutine(WaitUntilArrived());
    }

    private IEnumerator WaitUntilArrived()
    {
        yield return new WaitUntil(HasArrivedToDestination);
        End();
    }

    private bool HasArrivedToDestination()
    {
        return seeker.reachedDestination;
    }

}
