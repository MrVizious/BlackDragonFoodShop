using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ExtensionMethods;
using UnityEngine.InputSystem;
using DesignPatterns;

[RequireComponent(typeof(Rigidbody2D))]
public class MovingState : PlayerState
{
    private Rigidbody2D rb;
    public override void Enter(StateMachine<PlayerState> newStateMachine)
    {
        //Debug.Log("Entering moving state");
        base.Enter(newStateMachine);
        rb = playerController.rb;
    }

    private void Update()
    {
        if (playerController.lastMovementInput.sqrMagnitude < 0.1f)
        {
            playerController.ChangeToState(this.GetOrAddComponent<IdleState>());
            return;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition((Vector2)rb.position + playerController.lastMovementInput * playerData.speed * Time.deltaTime);
    }

}
