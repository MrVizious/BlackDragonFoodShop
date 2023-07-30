using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Trash : MonoBehaviour
{

    public float secondsUntilMaximumAnnoyance = 3f;
    private float _accumulatedAnnoyance = 0f;
    private float accumulatedAnnoyance
    {
        get => _accumulatedAnnoyance;
        set
        {
            value = Mathf.Clamp01(value);
            _accumulatedAnnoyance = value;
        }
    }
    private void Update()
    {
        if (accumulatedAnnoyance >= 1f) return;
        float addedAmount = Time.deltaTime * 1f / secondsUntilMaximumAnnoyance;
        LevelManager.Instance.trashPoints -= accumulatedAnnoyance;
        accumulatedAnnoyance += addedAmount;
        LevelManager.Instance.trashPoints += accumulatedAnnoyance;
    }
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
        InteractionController interactionController = other.GetComponent<InteractionController>();
        if (interactionController == null) return;
        interactionController.RemoveInteraction(this);
    }

    private void CollisionHappenning(Collider2D other)
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

    private void OnDestroy()
    {
        LevelManager.Instance.trashPoints -= accumulatedAnnoyance;
    }

}