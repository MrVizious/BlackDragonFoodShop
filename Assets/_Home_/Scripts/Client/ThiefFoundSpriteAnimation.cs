using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThiefFoundSpriteAnimation : MonoBehaviour
{
    private float blinkingSpeed = 8f;
    private Color targetColor = Color.red;
    private Color initialColor;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer spriteRenderer
    {
        get
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            return _spriteRenderer;
        }
    }

    private void Awake()
    {
        initialColor = spriteRenderer.color;
    }
    void Update()
    {
        spriteRenderer.color = Color.Lerp(initialColor, targetColor, Mathf.Sin(Time.time * blinkingSpeed));
    }
}
