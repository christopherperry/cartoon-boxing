using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Boxer : MonoBehaviour
{
    private BoxingInputActions boxingInputActions;

    public int totalHealth = 50;
    public float movementSpeed;
    public bool movementEnabled = true;
    public LayerMask opponentLayerMask;
    public Collider2D leftPunchTrigger;
    public Collider2D rightPunchTrigger;
    public Collider2D upPunchTrigger;

    private Animator animator;
    private Rigidbody2D rigidbody;

    private bool isBlocking;
    private bool isPunchLeft;
    private bool isPunchRight;
    private Vector2 movement;

    private bool canPunch;
    private ContactFilter2D contactFilter;

    private void Awake()
    {
        boxingInputActions = new BoxingInputActions();
        animator = GetComponent<Animator>();
        rigidbody = GetComponent<Rigidbody2D>();

        contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = opponentLayerMask;
        contactFilter.useTriggers = false;

        // Blocking
        boxingInputActions.Boxer.Block.started += ctx => isBlocking = true;
        boxingInputActions.Boxer.Block.canceled += ctx => isBlocking = false;

        // Moving
        boxingInputActions.Boxer.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        boxingInputActions.Boxer.Move.canceled += ctx => movement = Vector2.zero;

        // Punch Left
        boxingInputActions.Boxer.PunchLeft.started += OnPunchLeftStarted;
        boxingInputActions.Boxer.PunchLeft.canceled += OnPunchLeftCanceled;
        // Punch Right
        boxingInputActions.Boxer.PunchRight.started += OnPunchRightStarted;
        boxingInputActions.Boxer.PunchRight.canceled += OnPunchRightCanceled;
    }

    private void OnEnable()
    {
        if (movementEnabled)
            boxingInputActions.Enable();
    }

    private void OnDisable()
    {
        boxingInputActions.Disable();
    }

    private void OnPunchLeftStarted(CallbackContext ctx)
    {
        if (transform.localScale.x > 0)
        {
            isPunchLeft = true;
        }
        else
        {
            isPunchRight = true;
        }
    }

    private void OnPunchLeftCanceled(CallbackContext ctx)
    {
        if (transform.localScale.x > 0)
        {
            isPunchLeft = false;
        }
        else
        {
            isPunchRight = false;
        }
    }

    private void OnPunchRightStarted(CallbackContext ctx)
    {
        if (transform.localScale.x > 0)
        {
            isPunchRight = true;
        }
        else
        {
            isPunchLeft = true;
        }
    }

    private void OnPunchRightCanceled(CallbackContext ctx)
    {
        if (transform.localScale.x > 0)
        {
            isPunchRight = false;
        }
        else
        {
            isPunchLeft = false;
        }
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
        else if (canPunch && isPunchRight)
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

    public void ReceivePunch(int damage)
    {
        totalHealth -= damage;
        animator.SetTrigger("hurt");
    }

    private void OnLeftPunch()
    {
        Collider2D collider = leftPunchTrigger;
        if (transform.localScale.x < 0)
        {
            collider = rightPunchTrigger;
        }

        OnPunch(collider);
    }

    private void OnRightPunch()
    {
        Collider2D collider = rightPunchTrigger;
        if (transform.localScale.x < 0)
        {
            collider = leftPunchTrigger;
        }

        OnPunch(collider);
    }

    private void OnUpPunch()
    {
        OnPunch(upPunchTrigger);
    }

    private void OnPunch(Collider2D collider)
    {
        var hits = new List<Collider2D>();
        int numHits = collider.OverlapCollider(contactFilter, hits);
        if (numHits > 0)
        {
            hits[0].gameObject.GetComponent<Boxer>().ReceivePunch(1);
        }
    }
}
