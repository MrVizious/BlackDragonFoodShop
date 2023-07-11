using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ItemShelf : PointOfInterest
{
    [Header("Sprites")]
    public Sprite fullSprite;
    public Sprite halfSprite;
    public Sprite emptySprite;

    [Header("ItemCount")]
    public int maxItemCount = 3;
    [SerializeField]
    public int currentItemCount
    {
        get => _currentItemCount;
        set
        {
            if (value >= maxItemCount) spriteRenderer.sprite = fullSprite;
            else if (value <= 0) spriteRenderer.sprite = emptySprite;
            else spriteRenderer.sprite = halfSprite;
            _currentItemCount = value;
        }
    }
    [SerializeField]
    private int _currentItemCount;

    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer spriteRenderer
    {
        get
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
            return _spriteRenderer;
        }
    }

    private void Start()
    {
        currentItemCount = maxItemCount;
    }


}
