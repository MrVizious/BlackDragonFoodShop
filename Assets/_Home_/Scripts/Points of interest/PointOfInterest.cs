using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using TypeReferences;

public class PointOfInterest : MonoBehaviour
{
    public PointOfInterestPossibleEventsData possibleEventsData;
    public TypeReference GetEvent(bool isThief = false)
    {
        return possibleEventsData.GetEvent(isThief);
    }

}
