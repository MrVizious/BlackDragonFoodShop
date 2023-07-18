using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ItemShelf : PointOfInterest
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
}
