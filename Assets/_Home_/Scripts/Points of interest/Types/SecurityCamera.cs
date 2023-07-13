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
            if (_spriteRenderer == null) _spriteRenderer = GetComponent<SpriteRenderer>();
            return _spriteRenderer;
        }
    }
    private PlayerInput _input;
    private PlayerInput input
    {
        get
        {
            if (_input == null) _input = FindObjectOfType<PlayerInput>();
            return _input;
        }
    }
    [OnValueChanged("UpdateWorking")]
    [SerializeField]
    private bool isWorking = true;
    [SerializeField]
    private bool isManned = false;

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
        input.actions["Interact"].performed += ctx => Repair();
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
        if (isManned) SetWorking(true);
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag.ToLower().Equals("player"))
        {
            isManned = true;
        }
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag.ToLower().Equals("player"))
        {
            isManned = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag.ToLower().Equals("player"))
        {
            isManned = false;
        }
    }
}
