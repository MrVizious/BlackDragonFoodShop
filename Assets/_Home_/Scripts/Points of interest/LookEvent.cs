using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;

public class LookEvent : QueueableEvent
{
    public float secondsToLook = 3f;
    public override void Setup(EventQueue newQueue)
    {
        base.Setup(newQueue);
        secondsToLook = Random.Range(2f, 5f);
        Debug.Log(secondsToLook);
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
        queue.ExecuteNextEvent();

        LookEvent lookEvent = gameObject.AddComponent<LookEvent>();
        queue.AddEvent(lookEvent);
        queue.ExecuteNextEvent();
    }


}
