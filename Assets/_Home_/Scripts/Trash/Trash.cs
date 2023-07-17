using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Trash : MonoBehaviour
{

    private PlayerInput _input;
    private PlayerInput input
    {
        get
        {
            if (_input == null) _input = FindObjectOfType<PlayerInput>();
            return _input;
        }
    }

    [SerializeField]
    private PlayerController player = null;

    private void Start()
    {
        input.actions["Interact"].performed += ctx => Pickup();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        PlayerController newPlayer = other.GetComponent<PlayerController>();
        if (newPlayer == null) return;
        player = newPlayer;
    }
    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerController newPlayer = other.GetComponent<PlayerController>();
        if (newPlayer == null) return;
        player = newPlayer;
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        PlayerController newPlayer = other.GetComponent<PlayerController>();
        if (newPlayer == player) return;
        player = null;
    }


    public void Pickup()
    {
        if (player == null) return;
        player.currentTrashAmount++;
        Destroy(gameObject);
    }
}