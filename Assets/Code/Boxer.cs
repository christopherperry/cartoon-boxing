using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Boxer : MonoBehaviour
{
    public enum BoxerName
    {
        Red,
        Blue,
        None
    }

    public BoxerName name;
    public GameEvent knockoutEvent;

    public FloatVariable maxHealth;
    public FloatVariable currentHealth;
    public GameEvent onHealthChange;

    public FloatVariable maxHearts;
    public FloatVariable currentHearts;
    public GameEvent onHeartsChange;

    public float dizzyTimeSeconds = 2f;
    public int heartsLostPerBlock = 3;
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
    private SpriteRenderer spriteRenderer;

    private bool isBlocking;
    private bool isPunching;
    private bool isKnockedOut;
    private bool isInFlightMode;
    private bool isDizzy;
    private Vector2 movement;

    private Vector2 startingPosition;
    private bool canBlock = true;
    private ContactFilter2D contactFilter;

    private void Awake()
    {
        currentHealth.Value = maxHealth.Value;
        currentHearts.Value = maxHearts.Value;

        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        startingPosition = transform.position;

        contactFilter = new ContactFilter2D();
        contactFilter.useLayerMask = true;
        contactFilter.layerMask = opponentLayerMask;
        contactFilter.useTriggers = false;
    }

    private void Start()
    {
        var gamepad = GetGamepad();
        if (gamepad != null)
        {
            gamepad.ResetHaptics();
        }

        onHeartsChange.Raise();
    }

    public void MoveToStartingPosition()
    {
        transform.position = startingPosition;
        rigidbody.velocity = Vector2.zero;

        isBlocking = false;
        isPunching = false;
        isDizzy = false;
        isInFlightMode = false;
        isKnockedOut = false;
    }

    public void ResetHealth()
    {
        currentHealth.Value += (maxHealth.Value - currentHealth.Value) / 2f;
        currentHearts.Value = maxHearts.Value;

        onHealthChange.Raise();
        onHeartsChange.Raise();
    }

    public BoxerName GetName()
    {
        return name;
    }

    public bool IsKnockedOut()
    {
        return isKnockedOut;
    }

    #region Input Action Handling

    private void OnMove(InputValue inputValue)
    {
        movement = inputValue.Get<Vector2>();
    }

    private void OnBlock(InputValue inputValue)
    {
        if (!canBlock || isInFlightMode || isDizzy) return;

        isBlocking = inputValue.isPressed;

        animator.SetBool("block", isBlocking);

        if (!isBlocking && !tauntTrigger.IsTouchingLayers(opponentLayerMask) && Mathf.Abs(movement.x) == 0)
        {
            audioSource.PlayOneShot(tauntClip);
        }
    }

    private void OnPunchLeft(InputValue inputValue)
    {
        if (isPunching || isBlocking || isInFlightMode || isDizzy) return;

        isPunching = true;

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

    private void OnPunchRight(InputValue inputValue)
    {
        if (isPunching || isBlocking || isInFlightMode || isDizzy) return;

        isPunching = true;

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

    private bool isBeingShoved;

    private void FixedUpdate()
    {
        if (isBeingShoved) return;

        if (isKnockedOut || isBlocking || isPunching || isDizzy)
        {
            rigidbody.velocity = Vector2.zero;
            animator.SetFloat("velocity", 0f);
        }
        else
        {
            rigidbody.velocity = new Vector2(movement.x * movementSpeed, 0f);
            animator.SetFloat("velocity", rigidbody.velocity.x * transform.localScale.x);
        }
    }

    #endregion

    #region Being Punched By Other Boxer

    public void ReceivePunch(Boxer otherBoxer, float damage, AudioClip hurtClip)
    {
        if (isKnockedOut) return;

        if (isBlocking)
        {
            LoseHearts(heartsLostPerBlock);
            otherBoxer.OnPunchBlocked();
            audioSource.PlayOneShot(punchBlockedClip);
            return;
        }
        
        currentHealth.Value -= damage;
        onHealthChange.Raise();

        if (damage >= 1)
        {
            RumbleGamepadLow();
            StartCoroutine(ShoveBoxerBack(2f));
        }
        else
        {
            ShoveBoxerBack(0.25f);
            RumbleGamepadHigh();
        }

        if (currentHealth.Value <= 0)
        {
            Time.timeScale = 0.33f;
            isKnockedOut = true;
            spriteRenderer.sortingOrder = spriteRenderer.sortingOrder - 1;
            animator.SetBool("ko", true);
            audioSource.PlayOneShot(knockoutClip);
            knockoutEvent.Raise();
        }
        else if (isPunching)
        {
            StartCoroutine(BeDizzy());
        }
        else
        {
            animator.SetTrigger("hurt");
            audioSource.PlayOneShot(hurtClip);
        }

        isInFlightMode = false;
        isPunching = false;
    }

    private IEnumerator ShoveBoxerBack(float forceMagnitude)
    {
        isBeingShoved = true;
        rigidbody.AddForce(new Vector2(-transform.localScale.x * forceMagnitude, 0f), ForceMode2D.Impulse);
        while (Mathf.Abs(rigidbody.velocity.x) > 0.01f)
        {
            yield return null;
        }
        isBeingShoved = false;
    }

    #endregion

    private void LoseHearts(int numHearts)
    {
        currentHearts.Value -= numHearts;
        currentHearts.Value = Mathf.Max(0, currentHearts.Value);
        onHeartsChange.Raise();

        if (currentHearts.Value <= 0)
        {
            StartCoroutine(FlightNotFightMode());
        }
    }

    private IEnumerator FlightNotFightMode()
    {
        isInFlightMode = true;
        canBlock = false;

        animator.SetBool("block", false);
        spriteRenderer.color = new Color(0f, 255f, 218f);
        yield return new WaitForSeconds(dizzyTimeSeconds);
        spriteRenderer.color = Color.white;

        isInFlightMode = false;
        canBlock = true;

        currentHearts.Value = maxHearts.Value;
        onHeartsChange.Raise();
    }

    #region Stuff that's called from the Animator

    public void OnPunchEnd()
    {
        isPunching = false;
    }

    private void SetDead()
    {
        Time.timeScale = 1f;
        rigidbody.isKinematic = true;
        animator.SetBool("ko", false);
        animator.SetBool("dead", true);
    }

    // Left is a jab
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
            OnPunch(collider, leftHurtClip, 0.25f);
        }
    }

    // Right is a straight
    private void OnRightPunch()
    {
        Collider2D collider = rightPunchTrigger;
        if (transform.localScale.x < 0)
        {
            collider = leftPunchTrigger;
            OnPunch(collider, leftHurtClip, 0.25f);
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

    private void OnPunch(Collider2D collider, AudioClip hurtClip, float damage)
    {
        var hits = new List<Collider2D>();
        int numHits = collider.OverlapCollider(contactFilter, hits);
        if (numHits > 0)
        {
            if (damage >= 1)
            {
                RumbleGamepadLow();
                StartCoroutine(ShoveBoxerBack(1f));
            }
            else
            {
                RumbleGamepadHigh();
            }

            
            hits[0].gameObject.GetComponentInParent<Boxer>().ReceivePunch(this, damage, hurtClip);
        }
    }

    public void OnPunchBlocked()
    {
        LoseHearts(1);
    }

    #endregion

    private IEnumerator BeDizzy()
    {
        isDizzy = true;
        animator.SetBool("dizzy", true);
        audioSource.PlayOneShot(dizzyClip);
        yield return new WaitForSecondsRealtime(dizzyTimeSeconds);
        animator.SetBool("dizzy", false);
        isDizzy = false;
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
        var gamepad = GetGamepad();
        if (gamepad == null) yield return null;

        gamepad.SetMotorSpeeds(0f, 0.75f);
        yield return new WaitForSecondsRealtime(0.2f);
        gamepad.ResetHaptics();
    }

    private IEnumerator GamepadRumbleLowCoroutine()
    {
        var gamepad = GetGamepad();
        if (gamepad == null) yield return null;

        gamepad.SetMotorSpeeds(0.75f, 0f);
        yield return new WaitForSecondsRealtime(0.2f);
        gamepad.ResetHaptics();
    }

    private Gamepad GetGamepad()
    {
        var devices = GetComponent<PlayerInput>().user.pairedDevices;
        foreach (var device in devices)
        {
            var gamepad = device as Gamepad;
            if (gamepad != null) return gamepad;
        }

        return null;
    }
}
