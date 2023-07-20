using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCarrier : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer carryItemSpriteRenderer;

    [Header("Sprites")]
    public Sprite replenishmentSprite;
    public Sprite trashSprite;

    public int currentTrashAmount
    {
        get => _currentTrashAmount;
        set
        {
            _currentTrashAmount = value;
            if (currentTrashAmount <= 0)
            {
                if (carryingReplenishment == false) carryItemSpriteRenderer.sprite = null;
                return;
            }

            carryItemSpriteRenderer.sprite = trashSprite;
        }
    }
    public bool carryingReplenishment
    {
        get => _carryingReplenishment;
        set
        {
            _carryingReplenishment = value;
            if (carryingReplenishment == false)
            {
                if (currentTrashAmount <= 0) carryItemSpriteRenderer.sprite = null;
                return;
            }

            carryItemSpriteRenderer.sprite = replenishmentSprite;
        }
    }
    private int _currentTrashAmount = 0;
    private bool _carryingReplenishment = false;
}
