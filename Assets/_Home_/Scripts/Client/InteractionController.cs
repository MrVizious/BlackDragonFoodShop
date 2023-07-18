using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionController : MonoBehaviour
{
    public int selectedIndex
    {
        get => _selectedIndex;
        set
        {
            _selectedIndex = Mathf.Clamp(value, 0, interactions.Count);
            for (int i = 0; i < interactions.Count; i++)
            {
                if (i == selectedIndex) interactions[i].behaviour.GetComponentInChildren<SpriteRenderer>().material.SetFloat("_Active", 1f);
                else interactions[i].behaviour.GetComponentInChildren<SpriteRenderer>().material.SetFloat("_Active", 0f);
            }
        }
    }
    [SerializeField]
    private List<Interaction> interactions = new List<Interaction>();
    private int _selectedIndex = 0;
    private PlayerInput _input;
    private PlayerInput input
    {
        get
        {
            if (_input == null) _input = FindObjectOfType<PlayerInput>();
            return _input;
        }
    }
    private void Start()
    {
        input.actions["Interact"].started += ctx => Interact(ctx);
    }

    public void Interact(InputAction.CallbackContext ctx)
    {
        if (interactions.Count <= 0) return;
        if (selectedIndex < 0 || selectedIndex >= interactions.Count) return;
        interactions[selectedIndex].action.Invoke();
        RemoveInteraction(interactions[selectedIndex].behaviour);
        if (selectedIndex >= interactions.Count) selectedIndex--;
    }

    public void AddInteraction(MonoBehaviour behaviour, System.Action action)
    {
        foreach (Interaction interaction in interactions)
        {
            if (interaction.behaviour == behaviour) return;
        }
        Interaction newInteraction = new Interaction();
        newInteraction.behaviour = behaviour;
        newInteraction.action = action;
        interactions.Add(newInteraction);
        selectedIndex = selectedIndex;
    }

    public void RemoveInteraction(MonoBehaviour behaviour)
    {
        behaviour.GetComponentInChildren<SpriteRenderer>().material.SetFloat("_Active", 0f);
        for (int i = 0; i < interactions.Count; i++)
        {
            if (interactions[i].behaviour == behaviour)
            {
                interactions.RemoveAt(i);
                if (selectedIndex >= interactions.Count)
                {
                    selectedIndex--;
                }
                return;
            }
        }
    }

    [System.Serializable]
    private struct Interaction
    {
        public MonoBehaviour behaviour;
        public System.Action action;
    }
}
