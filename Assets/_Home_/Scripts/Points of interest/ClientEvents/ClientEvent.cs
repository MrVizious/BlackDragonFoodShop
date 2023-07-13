using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using Cysharp.Threading.Tasks;
using UtilityMethods;

public abstract class ClientEvent : QueueableEvent
{
    public Client client;
    public PointOfInterest pointOfInterest;
    protected float durationInSeconds;
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
}
