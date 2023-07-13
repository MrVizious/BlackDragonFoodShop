using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using RuntimeSet;

public class PointsOfInterestPopulator : MonoBehaviour
{
    public List<PointOfInterest> allPointsOfInterest = new List<PointOfInterest>();
    public RuntimeSetPointOfInterest activePointsOfInterest;

    private void Awake()
    {
        activePointsOfInterest.Items.Clear();
        if (activePointsOfInterest == null) return;
        allPointsOfInterest = new List<PointOfInterest>(FindObjectsOfType<PointOfInterest>());
        for (int i = 0; i < allPointsOfInterest.Count; i++)
        {
            PointOfInterest poi = allPointsOfInterest[i];
            if (poi.enabled) activePointsOfInterest.Add(poi);
            poi.onDisable.AddListener(() => RemoveFromActive(poi));
            poi.onEnable.AddListener(() => AddToActive(poi));
        }
    }

    public void RemoveFromActive(PointOfInterest poi)
    {
        activePointsOfInterest?.Remove(poi);
    }
    public void AddToActive(PointOfInterest poi)
    {
        activePointsOfInterest?.Add(poi);
    }
}
