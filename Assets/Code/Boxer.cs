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
    public Collider2D tauntTrigger;
    public AudioClip leftHurtClip;
    public AudioClip rightHurtClip;
    public AudioClip upHurtClip;
    public AudioClip dizzyClip;
    public AudioClip knockoutClip;
    public AudioClip punchBlockedClip;
    public AudioClip tauntClip;
    public AudioClip punchThrow;
    public AudioClip heavyPunchThrow;

    private Animator animator;
    private AudioSource audioSource;
    private Rigidbody2D rigidbody;

    private bool isBlocking;
    private Vector2 movement;

    private bool canPunch;
    private ContactFilter2D contactFilter;

    private void Awake()
    {
        boxingInputActions = new BoxingInputActions();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody2D>();

        contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = opponentLayerMask;
        contactFilter.useTriggers = false;

        // Blocking
        boxingInputActions.Boxer.Block.started += OnBlockStarted;
        boxingInputActions.Boxer.Block.canceled += OnBlockCanceled;

        // Moving
        boxingInputActions.Boxer.Move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        boxingInputActions.Boxer.Move.canceled += ctx => movement = Vector2.zero;

        // Punch Left
        boxingInputActions.Boxer.PunchLeft.started += OnPunchLeftStarted;

        // Punch Right
        boxingInputActions.Boxer.PunchRight.started += OnPunchRightStarted;
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

    private void OnBlockStarted(CallbackContext ctx)
    {
        isBlocking = true;
        animator.SetBool("block", isBlocking);

        if (!tauntTrigger.IsTouchingLayers(opponentLayerMask))
        {
            audioSource.PlayOneShot(tauntClip);
        }
    }

    private void OnBlockCanceled(CallbackContext ctx)
    {
        isBlocking = false;
        animator.SetBool("block", isBlocking);
    }

    private void OnPunchLeftStarted(CallbackContext ctx)
    {
        if (transform.localScale.x > 0)
        {
            OnLeftPunchAction();
        }
        else
        {
            OnRightPunchAction();
        }
    }

    private void OnPunchRightStarted(CallbackContext ctx)
    {
        if (movement.y > 0)
        {
            OnUpPunchAction();
        }
        else if (transform.localScale.x > 0)
        {
            OnRightPunchAction();
        }
        else
        {
            OnLeftPunchAction();
        }
    }

    private void OnLeftPunchAction()
    {
        animator.SetTrigger("punch-left");
        audioSource.PlayOneShot(punchThrow);
    }

    private void OnRightPunchAction()
    {
        animator.SetTrigger("punch-right");
        audioSource.PlayOneShot(punchThrow);
    }

    private void OnUpPunchAction()
    {
        animator.SetTrigger("punch-up");
        audioSource.PlayOneShot(heavyPunchThrow);
    }

    private void FixedUpdate()
    {
        rigidbody.velocity = new Vector2(movement.x * movementSpeed, 0f);
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

    public void ReceivePunch(int damage, AudioClip hurtClip)
    {
        if (isBlocking)
        {
            audioSource.PlayOneShot(punchBlockedClip);
        }
        else
        {
            totalHealth -= damage;
            animator.SetTrigger("hurt");
            audioSource.PlayOneShot(hurtClip);
        }
    }

    private void OnLeftPunch()
    {
        Collider2D collider = leftPunchTrigger;
        if (transform.localScale.x < 0)
        {
            collider = rightPunchTrigger;
            OnPunch(collider, rightHurtClip);
        }
        else
        {
            OnPunch(collider, leftHurtClip);
        }
    }

    private void OnRightPunch()
    {
        Collider2D collider = rightPunchTrigger;
        if (transform.localScale.x < 0)
        {
            collider = leftPunchTrigger;
            OnPunch(collider, leftHurtClip);
        }
        else
        {
            OnPunch(collider, rightHurtClip);
        }
    }

    private void OnUpPunch()
    {
        OnPunch(upPunchTrigger, upHurtClip);
    }

    private void OnPunch(Collider2D collider, AudioClip hurtClip)
    {
        var hits = new List<Collider2D>();
        int numHits = collider.OverlapCollider(contactFilter, hits);
        if (numHits > 0)
        {
            hits[0].gameObject.GetComponentInParent<Boxer>().ReceivePunch(1, hurtClip);
        }
    }
}
