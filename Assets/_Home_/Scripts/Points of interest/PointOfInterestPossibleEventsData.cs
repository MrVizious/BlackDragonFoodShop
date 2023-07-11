using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializableDictionaries;
using DesignPatterns;
using TypeReferences;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(fileName = "PointOfInterestPossibleEventsData", menuName = "Black Dragon Food Shop/PointOfInterestPossibleEventsData", order = 0)]
public class PointOfInterestPossibleEventsData : ScriptableObject
{
    public List<PossibleEventPercentage> clientEvents = new List<PossibleEventPercentage>();
    public List<PossibleEventPercentage> thiefEvents = new List<PossibleEventPercentage>();

    public TypeReference GetEvent(bool isThief = false)
    {
        if (isThief)
        {
            return GetEventFromList(thiefEvents);
        }
        else
        {
            return GetEventFromList(clientEvents);
        }
    }

    private int TotalPercentage(List<PossibleEventPercentage> events)
    {
        int counter = 0;
        foreach (PossibleEventPercentage eventPercentage in events)
        {
            counter += eventPercentage.probability;
        }
        return counter;

    }

    private TypeReference GetEventFromList(List<PossibleEventPercentage> events)
    {

        if (events == null || events.Count <= 0) return null;
        int maxPercentage = TotalPercentage(events);
        if (maxPercentage <= 0) return null;
        int randomPercentage = Random.Range(0, maxPercentage + 1);
        int counter = 0;
        foreach (PossibleEventPercentage eventPercentage in events)
        {
            if (randomPercentage <= (counter + eventPercentage.probability))
            {
                return eventPercentage.eventType;
            }
            else
            {
                counter += eventPercentage.probability;
            }
        }
        return null;
    }
}

[System.Serializable]
public class PossibleEventPercentage
{
    [Inherits(typeof(QueueableEvent))]
    public TypeReference eventType;

    public int probability;
}