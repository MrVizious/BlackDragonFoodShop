using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

public class PickUpZone : PointOfInterest
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        CollisionHappenning(other);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        CollisionHappenning(other);
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

    private void CollisionHappenning(Collider2D other)
    {
        if (other.tag.ToLower().Equals("player"))
        {
            InteractionController interactionController = other.GetComponent<InteractionController>();
            if (interactionController == null) return;
            ItemCarrier itemCarrier = other.GetComponent<ItemCarrier>();
            if (itemCarrier == null) return;
            if (itemCarrier.currentTrashAmount > 0) return;
            if (itemCarrier.carryingReplenishment) return;
            interactionController.AddInteraction(this, () => PickUpReplenishment(itemCarrier));
        }
    }

    public void PickUpReplenishment(ItemCarrier itemCarrier)
    {
        if (itemCarrier.carryingReplenishment) return;
        if (itemCarrier.currentTrashAmount > 0) return;
        itemCarrier.carryingReplenishment = true;
    }
}
