using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using DesignPatterns;
using TypeReferences;

public abstract class PointOfInterest : MonoBehaviour
{
    private bool _isActive = true;
    public bool isActive
    {
        get => _isActive;
        set
        {
            if (value)
            {
                onEnable.Invoke();
                _isActive = true;
            }
            else
            {
                onDisable.Invoke();
                _isActive = false;
            }
        }
    }
    public UnityEvent onDisable = new UnityEvent();
    public UnityEvent onEnable = new UnityEvent();
    public PointOfInterestPossibleEventsData possibleEventsData;
    public TypeReference GetEvent(bool isThief = false)
    {
        return possibleEventsData.GetEvent(isThief);
    }
}
