using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Sirenix.OdinInspector;

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
                if (i == selectedIndex)
                {
                    interactions[i].behaviour.GetComponentInChildren<SpriteRenderer>().material.SetFloat("_Active", 1f);
                    interactions[i].behaviour.GetComponentInChildren<SpriteRenderer>().sortingOrder = 10;

                }
                else
                {
                    interactions[i].behaviour.GetComponentInChildren<SpriteRenderer>().material.SetFloat("_Active", 0f);
                    interactions[i].behaviour.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
                }
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
        if (selectedIndex < interactions.Count)
        {
            RemoveInteraction(interactions[selectedIndex].behaviour);
        }
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
        behaviour.GetComponentInChildren<SpriteRenderer>().sortingOrder = 0;
        for (int i = 0; i < interactions.Count; i++)
        {
            if (interactions[i].behaviour == behaviour)
            {
                interactions.RemoveAt(i);
                if (selectedIndex >= interactions.Count - 1)
                {
                    selectedIndex--;
                }
                return;
            }
        }
    }

    [Button]
    public void ChooseNext(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;
        if (interactions.Count < 2) return;
        selectedIndex = (selectedIndex + 1) % interactions.Count;
    }

    [Button]
    public void ChoosePrevious(InputAction.CallbackContext ctx)
    {
        if (!ctx.started) return;
        if (interactions.Count < 2) return;
        if (selectedIndex == 0) selectedIndex = interactions.Count - 1;
        else selectedIndex--;
    }

    [System.Serializable]
    private struct Interaction
    {
        public MonoBehaviour behaviour;
        public System.Action action;
    }
}
