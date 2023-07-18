using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCarrier : MonoBehaviour
{
    [Header("References")]
    public SpriteRenderer carryItemSpriteRenderer;
    public GameObject dialogueBox;

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
                if (carryingReplenishment == false) dialogueBox.SetActive(false);
                return;
            }

            dialogueBox.SetActive(true);
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
                if (currentTrashAmount <= 0) dialogueBox.SetActive(false);
                return;
            }

            dialogueBox.SetActive(true);
            carryItemSpriteRenderer.sprite = replenishmentSprite;
        }
    }
    private int _currentTrashAmount = 0;
    private bool _carryingReplenishment = false;
}
