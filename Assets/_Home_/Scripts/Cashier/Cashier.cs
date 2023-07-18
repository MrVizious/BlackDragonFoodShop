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

    public void CustomerLeft(Client client)
    {
        for (int i = 0; i < spots.Count; i++)
        {
            if (spots[i].client == client)
            {
                UpdateClientsPositions(i);
                return;
            }
        }
    }

    [Button]
    public void RingUp()
    {
        Client client = spots[0].client;
        // There is no client to ring up
        if (client == null)
        {
            Debug.Log("No client to ring up");
            return;
        }

        if (!IsFirstClientAtCounter()) return;

        // Ring up
        LevelManager.Instance.points += client.currentNumberOfItems;
        LevelManager.Instance.rangUpClients++;
        client.currentNumberOfItems = 0;

        client.LeaveStore();
        UpdateClientsPositions();
    }

    private bool IsFirstClientAtCounter()
    {
        if (spots[0].client == null) return false;
        //The client is too far away
        return Vector2.Distance(spots[0].client.transform.position, transform.position) < 1f;
    }

    private void UpdateClientsPositions(int latestLeftCustomer = 0)
    {

        // Move each client to the next spot
        for (; latestLeftCustomer < spots.Count - 1; latestLeftCustomer++)
        {
            Spot spot = spots[latestLeftCustomer];
            if (latestLeftCustomer != 0 && spot.client == null) break;
            spot.client = spots[latestLeftCustomer + 1].client;
            spots[latestLeftCustomer] = spot;
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (spots[0].client == null) return;
        if (!IsFirstClientAtCounter()) return;
        if (other.tag.ToLower().Equals("player"))
        {
            InteractionController interactionController = other.GetComponent<InteractionController>();
            interactionController.AddInteraction(spots[0].client, RingUp);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (spots[0].client == null) return;
        if (!IsFirstClientAtCounter()) return;
        if (other.tag.ToLower().Equals("player"))
        {
            InteractionController interactionController = other.GetComponent<InteractionController>();
            interactionController.AddInteraction(spots[0].client, RingUp);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (spots[0].client == null) return;
        if (other.tag.ToLower().Equals("player"))
        {
            InteractionController interactionController = other.GetComponent<InteractionController>();
            interactionController.RemoveInteraction(spots[0].client);
        }
    }
}

[System.Serializable]
public struct Spot
{
    public Transform transform;
    public Client client;
}