using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

public class DropZone : PointOfInterest
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        CollisionHappening(other);
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        CollisionHappening(other);
    }

    private void CollisionHappening(Collider2D other)
    {
        if (other.tag.ToLower().Equals("player"))
        {
            InteractionController interactionController = other.GetComponent<InteractionController>();
            ItemCarrier itemCarrier = other.GetComponent<ItemCarrier>();
            if (itemCarrier == null || interactionController == null) return;
            if (itemCarrier.currentTrashAmount <= 0) return;
            interactionController.AddInteraction(this, () => DropTrash(itemCarrier));
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

    public void DropTrash(ItemCarrier itemCarrier)
    {
        if (itemCarrier.currentTrashAmount <= 0) return;
        if (itemCarrier.carryingReplenishment) return;
        itemCarrier.currentTrashAmount = 0;
    }
}
