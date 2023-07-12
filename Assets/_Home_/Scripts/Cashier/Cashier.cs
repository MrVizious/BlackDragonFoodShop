using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SerializableDictionaries;
using Sirenix.OdinInspector;
using Sirenix.Serialization;

[RequireComponent(typeof(Collider2D))]
public class Cashier : MonoBehaviour
{
    public List<Spot> spots = new List<Spot>();
    private void Start()
    {
        Debug.Log(spots.Count);
    }
    [System.Serializable]
    public struct Spot
    {
        public Transform transform;
        public Client client;
    }
}