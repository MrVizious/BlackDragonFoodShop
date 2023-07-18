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
        PlayerController player = other.GetComponent<PlayerController>();
        interactionController.AddInteraction(this, () => Pickup(player));

    }
    private void OnTriggerExit2D(Collider2D other)
    {
        InteractionController interactionController = other.GetComponent<InteractionController>();
        if (interactionController == null) return;
        interactionController.RemoveInteraction(this);
    }

    public void Pickup(PlayerController player)
    {
        if (player == null) return;
        player.currentTrashAmount++;
        Destroy(gameObject);
    }
}