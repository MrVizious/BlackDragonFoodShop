using System.Collections;
using System.Collections.Generic;
using DesignPatterns;
using UnityEngine;
using Sirenix.OdinInspector;
using ExtensionMethods;

public class Client : StateMachine<ClientState>
{
    public bool isSeen = false;
    private EventQueue _eventQueue;
    public EventQueue eventQueue
    {
        get
        {
            if (_eventQueue == null)
                _eventQueue = gameObject.GetOrAddComponent<EventQueue>();
            return _eventQueue;
        }
    }

    [Button]
    public void ChooseWalkingDestination()
    {
        List<PointOfInterest> pointsOfInterest = new List<PointOfInterest>(FindObjectsOfType<PointOfInterest>());
        PointOfInterest chosenPointOfInterest = pointsOfInterest[Random.Range(0, pointsOfInterest.Count)];

        WalkEvent walkEvent = gameObject.AddComponent<WalkEvent>();
        walkEvent.destinationSetter.target = chosenPointOfInterest.transform;
        Debug.Log(eventQueue);
        eventQueue.AddEvent(walkEvent);
        eventQueue.ExecuteNextEvent();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.ToLower().Equals("unmask"))
        {
            isSeen = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (isSeen) return;
        if (other.tag.ToLower().Equals("unmask"))
        {
            isSeen = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.ToLower().Equals("unmask"))
        {
            isSeen = false;
        }
    }
}
