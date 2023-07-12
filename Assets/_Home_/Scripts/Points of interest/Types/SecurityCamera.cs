using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SecurityCamera : PointOfInterest
{
    [OnValueChanged("UpdateWorking")]
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
    }
}