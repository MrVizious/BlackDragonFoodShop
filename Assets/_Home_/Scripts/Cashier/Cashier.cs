using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Collider2D))]
public class Cashier : MonoBehaviour
{
    public List<Spot> spots = new List<Spot>();
    [SerializeField]
    private bool isManned = false;

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
        if (!isManned) return;
        Client client = spots[0].client;
        // There is no client to ring up
        if (client == null)
        {
            Debug.Log("No client to ring up");
            return;
        }

        //The client is too far away
        if (Vector2.Distance(client.transform.position, transform.position) > 1f) return;

        // Ring up
        LevelManager.Instance.points += client.currentNumberOfItems;
        LevelManager.Instance.rangUpClients++;
        client.currentNumberOfItems = 0;

        client.LeaveStore();
        UpdateClientsPositions();
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
        if (other.tag.ToLower().Equals("player"))
        {
            isManned = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.ToLower().Equals("player"))
        {
            isManned = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.ToLower().Equals("player"))
        {
            isManned = false;
        }
    }
}

[System.Serializable]
public struct Spot
{
    public Transform transform;
    public Client client;
}