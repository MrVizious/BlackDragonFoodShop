using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using UnityEngine.InputSystem;

public class SecurityCamera : PointOfInterest
{
    public Sprite notWorkingSprite, workingSprite;
    private SpriteRenderer _spriteRenderer;
    private SpriteRenderer spriteRenderer
    {
        get
        {
            if (_spriteRenderer == null) _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            return _spriteRenderer;
        }
    }
    [SerializeField]
    private bool isWorking = true;

    private MeshRenderer _meshRenderer;
    private MeshRenderer meshRenderer
    {
        get
        {
            if (_meshRenderer == null) _meshRenderer = GetComponentInChildren<MeshRenderer>();
            return _meshRenderer;
        }
    }

    private void Start()
    {
        UpdateWorking();
    }

    private void UpdateWorking()
    {
        SetWorking(isWorking);
    }


    [Button]
    public void SetWorking(bool newValue)
    {
        isWorking = newValue;
        meshRenderer.gameObject.SetActive(isWorking);
        if (isWorking)
        {
            spriteRenderer.sprite = workingSprite;
        }
        else
        {
            spriteRenderer.sprite = notWorkingSprite;
        }
    }


    public void Repair()
    {
        SetWorking(true);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isWorking) return;
        if (other.tag.ToLower().Equals("player"))
        {
            InteractionController interactionController = other.GetComponent<InteractionController>();
            interactionController.AddInteraction(this, Repair);
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (isWorking) return;
        if (other.tag.ToLower().Equals("player"))
        {
            InteractionController interactionController = other.GetComponent<InteractionController>();
            interactionController.AddInteraction(this, Repair);
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (isWorking) return;
        if (other.tag.ToLower().Equals("player"))
        {
            InteractionController interactionController = other.GetComponent<InteractionController>();
            interactionController.RemoveInteraction(this);
        }
    }
}
