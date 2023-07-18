using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Trash : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D other)
    {
        InteractionController interactionController = other.GetComponent<InteractionController>();
        if (interactionController == null) return;
        ItemCarrier itemCarrier = other.GetComponent<ItemCarrier>();
        interactionController.AddInteraction(this, () => Pickup(itemCarrier));
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        InteractionController interactionController = other.GetComponent<InteractionController>();
        if (interactionController == null) return;
        interactionController.RemoveInteraction(this);
    }

    public void Pickup(ItemCarrier itemCarrier)
    {
        if (itemCarrier == null) return;
        itemCarrier.currentTrashAmount++;
        Destroy(gameObject);
    }
}