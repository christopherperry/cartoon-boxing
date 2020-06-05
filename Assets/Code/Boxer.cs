using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class Boxer : MonoBehaviour
{
    private BoxingInputActions boxingInputActions;

    public int maxHealth = 50;
    public float dizzyTimeSeconds = 2f;
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
    private bool isDead;
    private Vector2 movement;

    private bool canPunch;
    private int totalHealth;
    private ContactFilter2D contactFilter;

    private void Awake()
    {
        totalHealth = maxHealth;

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

    #region Input Action Handling

    private void OnBlockStarted(CallbackContext ctx)
    {
        isBlocking = true;
        animator.SetBool("block", isBlocking);

        if (!tauntTrigger.IsTouchingLayers(opponentLayerMask) && Mathf.Abs(movement.x) == 0)
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
        if (transform.localScale.x < 0 && movement.y > 0)
        {
            OnUpPunchAction();
        }
        else if (transform.localScale.x > 0)
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
        if (transform.localScale.x > 0 && movement.y > 0)
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

    #endregion

    #region Fixed Update

    private void FixedUpdate()
    {
        if (isDead || isBlocking)
        {
            rigidbody.velocity = Vector2.zero;
        }
        else
        {
            rigidbody.velocity = new Vector2(movement.x * movementSpeed, 0f);
        }

        animator.SetFloat("velocity", rigidbody.velocity.x * transform.localScale.x);
    }

    #endregion

    #region Being Punched By Other Boxer

    public void ReceivePunch(int damage, AudioClip hurtClip)
    {
        if (isBlocking)
        {
            audioSource.PlayOneShot(punchBlockedClip);
            return;
        }

        totalHealth -= damage;

        if (damage > 1)
        {
            RumbleGamepadLow();
        }
        else
        {
            RumbleGamepadHigh();
        }

        if (totalHealth <= 0)
        {
            Time.timeScale = 0.33f;
            animator.SetBool("ko", true);
            audioSource.PlayOneShot(knockoutClip);
        }
        else
        {
            animator.SetTrigger("hurt");
            audioSource.PlayOneShot(hurtClip);
        }
    }

    #endregion

    #region Stuff that's called from the Animator

    public void SetCanPunch()
    {
        canPunch = true;
    }

    public void SetCannotPunch()
    {
        canPunch = false;
    }

    private void SetDead()
    {
        Time.timeScale = 1f;
        isDead = true;
        rigidbody.isKinematic = true;
        animator.SetBool("ko", false);
        animator.SetBool("dead", true);
    }

    private void OnLeftPunch()
    {
        Collider2D collider = leftPunchTrigger;
        if (transform.localScale.x < 0)
        {
            collider = rightPunchTrigger;
            OnPunch(collider, rightHurtClip, 1);
        }
        else
        {
            OnPunch(collider, leftHurtClip, 1);
        }
    }

    private void OnRightPunch()
    {
        Collider2D collider = rightPunchTrigger;
        if (transform.localScale.x < 0)
        {
            collider = leftPunchTrigger;
            OnPunch(collider, leftHurtClip, 1);
        }
        else
        {
            OnPunch(collider, rightHurtClip, 1);
        }
    }

    private void OnUpPunch()
    {
        OnPunch(upPunchTrigger, upHurtClip, 2);
    }

    private void OnPunch(Collider2D collider, AudioClip hurtClip, int damage)
    {
        var hits = new List<Collider2D>();
        int numHits = collider.OverlapCollider(contactFilter, hits);
        if (numHits > 0)
        {
            if (damage > 1)
            {
                RumbleGamepadLow();
            }
            else
            {
                RumbleGamepadHigh();
            }

            hits[0].gameObject.GetComponentInParent<Boxer>().ReceivePunch(damage, hurtClip);
        }
    }

    #endregion

    private IEnumerator BeDizzy()
    {
        animator.SetBool("dizzy", true);
        audioSource.PlayOneShot(dizzyClip);
        yield return new WaitForSecondsRealtime(2f);
        animator.SetBool("dizzy", false);
    }

    private void RumbleGamepadHigh()
    {
        StartCoroutine(GamepadRumbleHighCoroutine());
    }

    private void RumbleGamepadLow()
    {
        StartCoroutine(GamepadRumbleLowCoroutine());
    }

    private IEnumerator GamepadRumbleHighCoroutine()
    {
        Gamepad.current.SetMotorSpeeds(0f, 0.5f);
        yield return new WaitForSecondsRealtime(0.15f);
        Gamepad.current.ResetHaptics();
    }

    private IEnumerator GamepadRumbleLowCoroutine()
    {
        Gamepad.current.SetMotorSpeeds(0.75f, 0f);
        yield return new WaitForSecondsRealtime(0.2f);
        Gamepad.current.ResetHaptics();
    }
}
