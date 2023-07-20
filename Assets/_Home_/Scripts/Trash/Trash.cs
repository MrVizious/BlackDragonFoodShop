using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Trash : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        CollisionHappening(other);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        CollisionHappening(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        InteractionController interactionController = other.GetComponent<InteractionController>();
        if (interactionController == null) return;
        interactionController.RemoveInteraction(this);
    }

    private void CollisionHappening(Collider2D other)
    {
        if (other.tag.ToLower().Equals("player"))
        {
            InteractionController interactionController = other.GetComponent<InteractionController>();
            if (interactionController == null) return;
            ItemCarrier itemCarrier = other.GetComponent<ItemCarrier>();
            if (itemCarrier == null) return;
            if (itemCarrier.carryingReplenishment) return;
            interactionController.AddInteraction(this, () => Pickup(itemCarrier));
        }
    }

    public void Pickup(ItemCarrier itemCarrier)
    {
        if (itemCarrier == null) return;
        itemCarrier.currentTrashAmount++;
        Destroy(gameObject);
    }
}