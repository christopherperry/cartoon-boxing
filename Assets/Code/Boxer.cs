using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boxer : MonoBehaviour
{

    private BoxingInputActions boxingInputActions;
    public float movementSpeed;

    private Animator animator;
    private Rigidbody2D rigidbody;

    private bool isBlocking;
    private bool isPunchLeft;
    private bool isPunchRight;
    private Vector2 movement;

    private bool canPunch;

    private void Awake()
    {
        boxingInputActions = new BoxingInputActions();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();

        // Blocking
        boxingInputActions.Boxer.Block.started += ctx => isBlocking = true;
        boxingInputActions.Boxer.Block.canceled += ctx => isBlocking = false;

        // Moving
        boxingInputActions.Boxer.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        boxingInputActions.Boxer.Move.canceled += ctx => movement = Vector2.zero;

        // Punch Left
        boxingInputActions.Boxer.PunchLeft.started += ctx => isPunchLeft = true;
        boxingInputActions.Boxer.PunchLeft.canceled += ctx => isPunchLeft = false;

        // Punch Right
        boxingInputActions.Boxer.PunchRight.started += ctx => isPunchRight = true;
        boxingInputActions.Boxer.PunchRight.canceled += ctx => isPunchRight = false;
    }

    private void OnEnable()
    {
        boxingInputActions.Enable();
    }

    private void OnDisable()
    {
        boxingInputActions.Disable();
    }

    private void Update()
    {
        // Block
        animator.SetBool("block", isBlocking);

        // Punch Up
        if (canPunch && movement.y > 0)
        {
            if (isPunchLeft || isPunchRight)
            {
                animator.SetTrigger("punch-up");
            }
        }

        // Punch Left
        else if (canPunch && isPunchLeft)
        {
            animator.SetTrigger("punch-left");
        }

        // Punch Right
        else if (canPunch &&  isPunchRight)
        {
            animator.SetTrigger("punch-right");
        }
        
        // Move
        else
        {
            rigidbody.velocity = new Vector2(movement.x * movementSpeed, 0f);
        }

        animator.SetFloat("velocity", rigidbody.velocity.x * transform.localScale.x);
    }

    public void SetCanPunch()
    {
        canPunch = true;
    }

    public void SetCannotPunch()
    {
        canPunch = false;
    }
}
