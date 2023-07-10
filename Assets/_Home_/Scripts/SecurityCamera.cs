using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class SecurityCamera : MonoBehaviour
{
    [OnValueChanged("UpdateWorking")]
    public bool isWorking = true;


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
    public void SetWorking(bool newValue)
    {
        isWorking = newValue;
        meshRenderer.enabled = isWorking;
    }
}
