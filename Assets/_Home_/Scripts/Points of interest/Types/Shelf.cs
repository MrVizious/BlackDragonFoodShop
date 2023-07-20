using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class Shelf : PointOfInterest
{
    [Header("Sprites")]
    public ShelfItemLibrary spriteLibrary;

    [Header("ItemCount")]
    public int maxItemCount = 3;

    [SerializeField]
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private int itemIndex;

    [SerializeField]
    public int currentItemCount
    {
        get => _currentItemCount;
        set
        {
            if (value >= maxItemCount) spriteRenderer.sprite = spriteLibrary.GetFullSprite(itemIndex);
            else if (value <= 0) spriteRenderer.sprite = null;
            else spriteRenderer.sprite = spriteLibrary.GetHalfSprite(itemIndex);
            _currentItemCount = value;
        }
    }

    [SerializeField]
    private int _currentItemCount;


    private void Start()
    {
        itemIndex = spriteLibrary.GetRandomIndex();
        currentItemCount = maxItemCount;
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
            if (!itemCarrier.carryingReplenishment)
            {
                interactionController.RemoveInteraction(this);
                return;
            }
            if (currentItemCount >= maxItemCount) return;
            interactionController.AddInteraction(this, () => Replenish(itemCarrier));
        }
    }

    public void Replenish(ItemCarrier itemCarrier)
    {
        if (!itemCarrier.carryingReplenishment) return;
        itemCarrier.carryingReplenishment = false;
        currentItemCount = maxItemCount;
    }
}
