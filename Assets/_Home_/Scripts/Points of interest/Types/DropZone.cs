using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

public class DropZone : PointOfInterest
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        CollisionHappenning(other);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        CollisionHappenning(other);
    }

    private void CollisionHappenning(Collider2D other)
    {
        if (other.tag.ToLower().Equals("player"))
        {
            InteractionController interactionController = other.GetComponent<InteractionController>();
            if (interactionController == null) return;
            ItemCarrier itemCarrier = other.GetComponent<ItemCarrier>();
            if (itemCarrier == null) return;
            if (itemCarrier.currentTrashAmount <= 0 && itemCarrier.carryingReplenishment == false) return;
            interactionController.AddInteraction(this, () => Drop(itemCarrier));
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.ToLower().Equals("player"))
        {
            InteractionController interactionController = other.GetComponent<InteractionController>();
            if (interactionController == null) return;
            interactionController.RemoveInteraction(this);
        }
    }

    public void Drop(ItemCarrier itemCarrier)
    {
        if (itemCarrier.currentTrashAmount <= 0 && !itemCarrier.carryingReplenishment) return;
        if (itemCarrier.carryingReplenishment)
        {
            itemCarrier.carryingReplenishment = false;
            return;
        }
        if (itemCarrier.currentTrashAmount > 0)
        {
            itemCarrier.currentTrashAmount = 0;
            return;
        }
    }
}
