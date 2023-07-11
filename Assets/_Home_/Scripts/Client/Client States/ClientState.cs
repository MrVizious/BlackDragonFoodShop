using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using UnityEngine.InputSystem;

public abstract class ClientState : MonoBehaviour, State<ClientState>
{
    public StateMachine<ClientState> stateMachine
    {
        get;
        protected set;
    }


    public void Enter(StateMachine<ClientState> newStateMachine)
    {
        this.enabled = true;
        stateMachine = newStateMachine;
    }

    public void Exit()
    {
        this.enabled = false;
    }
}