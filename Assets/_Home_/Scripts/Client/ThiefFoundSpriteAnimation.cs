using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ThiefFoundSpriteAnimation : MonoBehaviour
{
    private float blinkingSpeed = 8f;
    private Color targetColor = Color.red;
    [SerializeField] private Color initialColor;
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
        initialColor = spriteRenderer.material.GetColor("_MainColor");
    }
    void Update()
    {
        Color newColor = Color.Lerp(initialColor, targetColor, Mathf.Sin(Time.time * blinkingSpeed));
        Debug.Log("Setting color");
        spriteRenderer.material.SetColor("_MainColor", newColor);
    }
    private void OnDestroy()
    {
        spriteRenderer.material.SetColor("_MainColor", initialColor);
    }
}
