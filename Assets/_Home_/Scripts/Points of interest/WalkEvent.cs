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

    public override void End()
    {
        ChooseWalkingDestination();
        base.End();
    }

    public void ChooseWalkingDestination()
    {
        List<PointOfInterest> pointsOfInterest = new List<PointOfInterest>(FindObjectsOfType<PointOfInterest>());
        PointOfInterest chosenPointOfInterest = pointsOfInterest[Random.Range(0, pointsOfInterest.Count)];

        WalkEvent walkEvent = gameObject.AddComponent<WalkEvent>();
        walkEvent.destinationSetter.target = chosenPointOfInterest.transform;
        queue.AddEvent(walkEvent);
    }

}
