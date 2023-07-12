using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Collider2D))]
public class Cashier : MonoBehaviour
{
    public List<Spot> spots = new List<Spot>();

    public Transform GetFreeSpot(Client client)
    {
        for (int i = 0; i < spots.Count; i++)
        {
            Spot spot = spots[i];
            if (spot.client == null)
            {
                spot.client = client;
                spots[i] = spot;
                return spot.transform;
            }
        }
        return null;
    }

    [Button]
    public void RingUp()
    {
        // There is no client to ring up
        if (spots[0].client == null) return;

        //The client is too far away
        if (Vector2.Distance(spots[0].client.transform.position, spots[0].transform.position) > 0.5f) return;

        // Ring up
        // TODO: Add points for each sold item
        // TODO: Don't actually kill the client
        Destroy(spots[0].client.gameObject);

        // Move each client to the next spot
        for (int i = 0; i < spots.Count - 1; i++)
        {
            Spot spot = spots[i];
            if (i != 0 && spot.client == null) break;
            spot.client = spots[i + 1].client;
            spots[i] = spot;
        }

        // Set last spot free
        Spot lastSpot = spots[spots.Count - 1];
        lastSpot.client = null;
        spots[spots.Count - 1] = lastSpot;

        // Change target position for clients
        foreach (Spot currentSpot in spots)
        {
            if (currentSpot.client == null) break;
            currentSpot.client.GetComponent<AIDestinationSetter>().target = currentSpot.transform;
        }
    }
}

[System.Serializable]
public struct Spot
{
    public Transform transform;
    public Client client;
}