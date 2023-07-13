using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using Pathfinding;

public abstract class ClientEvent : QueueableEvent
{
    public Client client;
    public PointOfInterest pointOfInterest;
    protected float durationInSeconds;
    protected AIPath _aiPath;
    protected AIPath aiPath
    {
        get
        {
            if (_aiPath == null) _aiPath = GetComponent<AIPath>();
            return _aiPath;
        }
    }
    protected AIDestinationSetter _destinationSetter;
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
        durationInSeconds = Random.Range(2f, 5f);
        if (pointOfInterest != null) pointOfInterest.isActive = false;
    }

    public override void End()
    {
        if (pointOfInterest != null) pointOfInterest.isActive = true;
        base.End();
    }
    public override void Cancel()
    {
        if (pointOfInterest != null) pointOfInterest.isActive = true;
        base.Cancel();
    }
    protected bool HasArrivedToDestination()
    {
        bool hasArrived = aiPath.reachedDestination;
        return hasArrived;
    }
}
