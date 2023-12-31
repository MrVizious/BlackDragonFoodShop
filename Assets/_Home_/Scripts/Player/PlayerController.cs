using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;
using ExtensionMethods;
using UnityEngine.InputSystem;
using DesignPatterns;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : StateMachine<PlayerState>
{

    [Header("Data")]
    public PlayerData playerData;


    [HideInInspector] public Rigidbody2D rb { get; private set; }
    [HideInInspector] public Vector2 lastMovementInput = Vector2.zero;



    private Animator _animator;
    private Animator animator
    {
        get
        {
            if (_animator == null) _animator = GetComponentInChildren<Animator>();
            return _animator;
        }
    }

    protected override void Awake()
    {
        base.Awake();
    }

    private void Start()
    {
        rb = this.GetOrAddComponent<Rigidbody2D>();
        ChangeToState(this.GetOrAddComponent<IdleState>());
    }

    public override void ChangeToPreviousState()
    {
        if (stateStack.Count <= 1)
        {
            ChangeToState(this.GetOrAddComponent<IdleState>());
        }
        else
        {
            base.ChangeToPreviousState();
        }
    }

    public void Movement(InputAction.CallbackContext c)
    {
        lastMovementInput = c.ReadValue<Vector2>();
        if (lastMovementInput.magnitude > 0f)
        {
            animator.SetBool("Walking", true);
            if (Mathf.Abs(lastMovementInput.x) >= Mathf.Abs(lastMovementInput.y))
            {
                if (lastMovementInput.x > 0) animator.SetInteger("Direction", 3);
                else animator.SetInteger("Direction", 2);
            }
            else
            {
                if (lastMovementInput.y > 0) animator.SetInteger("Direction", 0);
                else animator.SetInteger("Direction", 1);
            }
        }
        else
        {
            animator.SetBool("Walking", false);
        }

        currentState.Move(c);
    }
}
