using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DesignPatterns;
using UnityEngine.InputSystem;

public abstract class PlayerState : MonoBehaviour, State<PlayerState>
{
    public PlayerController playerController
    {
        get
        {
            return ((PlayerController)stateMachine);
        }
    }
    public StateMachine<PlayerState> stateMachine
    {
        get;
        protected set;
    }
    public virtual void Move(InputAction.CallbackContext c) { }
    protected PlayerData playerData;

    public virtual void Enter(StateMachine<PlayerState> newStateMachine)
    {
        this.enabled = true;
        stateMachine = newStateMachine;
        playerData = ((PlayerController)stateMachine).playerData;
    }

    public virtual void Exit()
    {
        this.enabled = false;
    }


}