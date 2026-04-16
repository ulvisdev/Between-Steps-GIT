using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

/*
------------------- NOTES -------------------

Can check jump animation also with y velocity, but need 2 animations - jump up and fall down.

*/

public class PlayerController : MonoBehaviour
{

    // =========================
    // Serialized Settings
    // =========================

    [Header("Movement")]
    //[SerializeField] private float speed = 6f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private float maxRunSpeed = 6f;
    [SerializeField] private float runAcceleration = 20f;
    [SerializeField] private float runDeceleration = 15f;
    [SerializeField] private bool _active = true;

    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance = 0.12f;
    [SerializeField] private Vector2 groundCheckOffset = new Vector2(0f, -0.5f);
    [SerializeField] private Transform groundCheckLEFT;
    [SerializeField] private Transform groundCheckRIGHT;
    [SerializeField] private LayerMask groundLayer;

    [Header("Jump Assist")]
    [SerializeField] private float coyoteTime = 0.2f;
    [SerializeField] private float jumpBufferTime = 0.2f;

    [Header("Dash")]
    [SerializeField] private float dashingVelocity = 14f;
    [SerializeField] private float dashingTime = 0.5f;

    [Header("Gravity / Falling")]
    [SerializeField] private float maxFallSpeed = 20f;
    [SerializeField] private float jumpHangTimeThreshold = 0.2f;
    [SerializeField] private float jumpHangGravityMult = 0.5f;
    [SerializeField] private float jumpHangMaxSpeedMult = 0.8f;
    [SerializeField] private float jumpHangAccelMult = 0.8f;

    [Header("Effects / References")]
    [SerializeField] private TilemapSwitch tilemapswitch;
    [SerializeField] private ParticleSystem smokeFX;

    [Header("Respawn System")]
    public Transform currentCheckpoint;

    // =========================
    // Runtime State
    // =========================

    private float targetSpeed;
    private float accelRate;
    private float XInput;
    private float YInput;
    private float dashTrigger;

    private float coyoteTimeCounter;
    private float jumpBufferCounter;
    private float defaultGravityScale;

    private bool isGrounded;
    private bool isFacingRight = true;
    public bool isDashing;
    private bool canDash = true;
    private bool dashPressed;
    private bool triggerWasPressedLastFrame;

    public bool isDead = false;

    private Vector2 dashingDir;

    private bool blockGameplayInput;
    private bool requireJumpReleaseAfterPause;
    private bool requireDashReleaseAfterPause;
    private bool hasStartedTimer = false;

    // =========================
    // Cached Components
    // =========================

    private Rigidbody2D rb;
    private Animator anim;
    private Gamepad activeGamepad;
    private Timer timer;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        timer = FindFirstObjectByType<Timer>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //tr = GetComponent<TrailRenderer>();

        defaultGravityScale = rb.gravityScale;

        Vector3 rotator = transform.eulerAngles;
        rotator.y = isFacingRight ? 0f : -180f;
        transform.eulerAngles = rotator;

    }

    void Update()
    {

        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (PauseMenuManager.isPaused || blockGameplayInput)
        {
            XInput = 0f;
            YInput = 0f;
            targetSpeed = 0f;
            jumpBufferCounter = 0f;
            dashPressed = false;
            return;
        }

        XInput = Input.GetAxisRaw("Horizontal");
        YInput = Input.GetAxisRaw("Vertical");
        var dashInput = Input.GetButtonDown("Dash") /*|| Input.GetKeyDown(KeyCode.Keypad4)*/;

        // JOYSTICK CONTROLS ---------------------------------------------------

        float dpadX = Input.GetAxisRaw("HorizontalDPad");
        float dpadY = Input.GetAxisRaw("VerticalDPad");

        float stickX = Input.GetAxisRaw("HorizontalStick");
        float stickY = Input.GetAxisRaw("VerticalStick");

        if (Mathf.Abs(stickX) < 0.6f) stickX = 0f; //fixes possible stick drifts
        if (Mathf.Abs(stickY) < 0.6f) stickY = 0f; //fixes possible stick drifts

        if (Mathf.Abs(stickX) > 0.1f) XInput = stickX;
        if (Mathf.Abs(stickY) > 0.1f) YInput = stickY;

        if (Mathf.Abs(dpadX) > 0.1f) XInput = dpadX;
        if (Mathf.Abs(dpadY) > 0.1f) YInput = dpadY;

        dashTrigger = Input.GetAxisRaw("DashTrigger");
        //dashTrigger2 = Input.GetAxisRaw("DashTrigger2");
        bool triggerPressedNow = dashTrigger > 0.2f /*|| dashTrigger2 > 0.2*/;
        dashPressed = triggerPressedNow && !triggerWasPressedLastFrame;
        triggerWasPressedLastFrame = triggerPressedNow;

        dashInput = dashInput || dashPressed;

        // block dash input during pause
        bool dashHeld = Input.GetButton("Dash") || triggerPressedNow;

        if (requireDashReleaseAfterPause)
        {
            if (!dashHeld)
                requireDashReleaseAfterPause = false;

            dashInput = false;
        }

        // PLAYER ACTIVE CHECK --------------------------------------------------
        if (!_active) return;

        // DASH -----------------------------------------------------------------

        if (dashInput && canDash)
        {
            isDashing = true;
            canDash = false;
            //tr.emitting = true;
            dashingDir = new Vector2(XInput, YInput);

            //new addition - makes gravity be 0 for the duration of dash
            rb.gravityScale = isDashing ? 0f : defaultGravityScale;

            //fix for dpad bug with dash
            // if (Mathf.Abs(dpadX) > 0.1f)
            // {
            //     dashingDir = new Vector2(dpadX, 0f);
            // }
            // else if (Mathf.Abs(dpadY) > 0.1f)
            // {
            //     dashingDir = new Vector2(0f, dpadY);
            // }
            // else
            // {
            //     dashingDir = new Vector2(XInput, YInput);
            // }

            //new fix for dpad dash direction
            Vector2 dpadInput = new Vector2(dpadX, dpadY);

            if (dpadInput.sqrMagnitude > 0.01f)
            {
                dashingDir = dpadInput.normalized;
            }
            else
            {
                dashingDir = new Vector2(XInput, YInput);
            }

            //controller rumble
            activeGamepad = Gamepad.current;

            if (activeGamepad != null)
            {
                activeGamepad.SetMotorSpeeds(0.35f, 0.75f);
            }

            //sound effect
            if (AudioManager.Instance != null && AudioManager.Instance.dashSFX != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.dashSFX);
            }

            smokeFX.Play();
            tilemapswitch.TilemapSwitcheroo(); //TILEMAP SWITCHEROOOO
            CameraShakeManager.Instance.Shake(1.75f, 0.15f); //CAMERA SHAKEEE

            if (dashingDir == Vector2.zero)
            {
                float facing = isFacingRight ? 1f : -1f;
                dashingDir = new Vector2(facing, 0f);
            }
            StartCoroutine(StopDashing());
        }

        //bool for animating with dash
        anim.SetBool("IsDashing", isDashing);

        if (isDashing)
        {
            rb.linearVelocity = dashingDir.normalized * dashingVelocity;
            return;
        }

        // MOVEMENT -----------------------------------------------------------------

        // CHECK IF GROUNDED ----------------------------------------------------

        //MIDDLE
        Vector2 rayOrigin = groundCheck != null ? (Vector2)groundCheck.position : (Vector2)transform.position + groundCheckOffset;
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, groundCheckDistance, groundLayer);
        //isGrounded = hit.collider != null && !(rb.linearVelocity.y > 0.01f);

        //LEFT
        Vector2 rayOriginLEFT = groundCheckLEFT != null ? (Vector2)groundCheckLEFT.position : (Vector2)transform.position + groundCheckOffset;
        RaycastHit2D hitLEFT = Physics2D.Raycast(rayOriginLEFT, Vector2.down, groundCheckDistance, groundLayer);

        //RIGHT
        Vector2 rayOriginRIGHT = groundCheckRIGHT != null ? (Vector2)groundCheckRIGHT.position : (Vector2)transform.position + groundCheckOffset;
        RaycastHit2D hitRIGHT = Physics2D.Raycast(rayOriginRIGHT, Vector2.down, groundCheckDistance, groundLayer);

        //bool hitEDGE = hitLEFT.collider || hitRIGHT.collider;
        //isGroundedEDGE = hitEDGE && !(rb.linearVelocity.y > 0.01f);

        isGrounded = (hit.collider != null
                        || hitLEFT.collider != null
                        || hitRIGHT.collider != null) && !(rb.linearVelocity.y > 0.01f);

        // ACCELERATION ---------------------------------------------------------

        targetSpeed = XInput * maxRunSpeed;

        if (Mathf.Abs(targetSpeed) > 0.01f)
            accelRate = runAcceleration;
        else
            accelRate = runDeceleration;

        // START TIMER ON FIRST MOVEMENT ----------------------------------------
        if (!hasStartedTimer && RunState.CurrentRunSpeedrunnerMode)
        {
            if (timer != null && Mathf.Abs(XInput) > 0.01f)
            {
                timer.SetTimerActive(true);
                hasStartedTimer = true;
            }
        }

        // COYOTE ---------------------------------------------------------------

        if (isGrounded /*&& isGroundedEDGE*/)
        {
            coyoteTimeCounter = coyoteTime;
            canDash = true;
        }
        else coyoteTimeCounter -= Time.deltaTime;

        // JUMP BUFFER ----------------------------------------------------------

        //previous jump buffer
        // if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.JoystickButton0))
        // {
        //     jumpBufferCounter = jumpBufferTime;
        // }
        // else
        // {
        //     jumpBufferCounter -= Time.deltaTime;
        // }

        bool jumpPressed = Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.JoystickButton0);
        bool jumpHeld = Input.GetButton("Jump") || Input.GetKey(KeyCode.JoystickButton0);

        // block jump input during pause
        if (requireJumpReleaseAfterPause)
        {
            if (!jumpHeld)
                requireJumpReleaseAfterPause = false;
        }
        else
        {
            if (jumpPressed)
            {
                jumpBufferCounter = jumpBufferTime;
            }
            else
            {
                jumpBufferCounter -= Time.deltaTime;
            }
        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            if (AudioManager.Instance != null && AudioManager.Instance.jumpSFX != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.jumpSFX);
            }

            jumpBufferCounter = 0f;
            coyoteTimeCounter = 0f;
            isGrounded = false;
        }

        // HIGHER JUMP ON HOLD ------------------------------------------------------

        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.4f); //change this 0.0f to lower the smallest jump possible

            coyoteTimeCounter = 0f;
        }

        // OLD SPRITE FLIP ----------------------------------------------------------

        //if (!isWallJumping)
        /*if (spriteRenderer != null)
        {
            if (XInput > .2f) spriteRenderer.flipX = false;
            if (XInput < -.2f) spriteRenderer.flipX = true;

            //if (rb.linearVelocity.y == 0) smokeFX.Play();
        }*/

        // ANIMATION VARIABLES ------------------------------------------------------

        if (anim != null)
        {
            anim.SetFloat("Speed", Mathf.Abs(XInput));
            anim.SetFloat("YSpeed", rb.linearVelocity.y);
            anim.SetBool("isGrounded", isGrounded);
            //anim.SetBool("isGroundedEDGE", isGroundedEDGE);
        }

        // FAST FALL ON JUMP -----------------------------------------------------

        float desiredGravity = defaultGravityScale;
        bool falling = rb.linearVelocity.y < -0.01f;

        if (falling)
        {
            desiredGravity = defaultGravityScale * 1.5f;
            //rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }

        // HOLD MID-AIR ----------------------------------------------------------

        bool inAir = !isGrounded /*&& !isGroundedEDGE*/;
        bool nearY0 = Mathf.Abs(rb.linearVelocity.y) < jumpHangTimeThreshold;
        bool inJumping = inAir && nearY0;

        if (inJumping)
        {
            desiredGravity = defaultGravityScale * jumpHangGravityMult;
            accelRate *= jumpHangAccelMult;
            targetSpeed *= jumpHangMaxSpeedMult;
        }

        rb.gravityScale = desiredGravity;

    }

    public void PlayRunSFX()
    {
        if (AudioManager.Instance != null && AudioManager.Instance.stepSFX != null)
        {
            AudioManager.Instance.PlaySFX(AudioManager.Instance.stepSFX);
        }
    }

    private void TurnCheck()
    {
        if (XInput > 0 && !isFacingRight) Turn();
        else if (XInput < 0 && isFacingRight) Turn();
    }

    private void Turn()
    {

        //flip by transforming the scale ? 1 : -1

        // isFacingRight = !isFacingRight;

        // Vector3 scale = transform.localScale;
        // scale.x *= -1f;
        // transform.localScale = scale;

        // flip by Y rotation ? 0 : 180

        if (isFacingRight)
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 180f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;
        }
    }

    public bool IsFacingRight()
    {
        return isFacingRight;
    }

    // OLD FIXEDUPDATE -------------------------------------------------------

    /*void FixedUpdate()
    {
        if (isDashing) return;
        rb.linearVelocity = new Vector2(XInput * speed, rb.linearVelocity.y);
    }*/

    void FixedUpdate()
    {

        if (isDead)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        //block player input while paused
        if (PauseMenuManager.isPaused || blockGameplayInput)
        {
            rb.linearVelocity = new Vector2(0f, rb.linearVelocity.y);
            return;
        }

        if (isDashing) return;

        float maxSpeedChange = accelRate * Time.fixedDeltaTime;
        float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, maxSpeedChange);

        rb.linearVelocity = new Vector2(newX, rb.linearVelocity.y);

        // CLAMP FALL SPEED ---------------------------------------------------

        if (rb.linearVelocity.y < -maxFallSpeed)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, -maxFallSpeed);

        // TURN PLAYER --------------------------------------------------------

        if (XInput > 0 || XInput < 0)
        {
            TurnCheck();
        }

    }

    // DASH ----------------------------------------------------------------
    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        //tr.emitting = false;
        isDashing = false;

        StopRumble();

        //stops dash momentum
        if (rb.linearVelocity.y > 0f)
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, defaultGravityScale * 1.5f);

    }

    private void StopRumble()
    {
        if (activeGamepad != null)
        {
            activeGamepad.SetMotorSpeeds(0f, 0f);
            activeGamepad = null;
        }
    }

    private void OnDisable()
    {
        StopRumble();
    }

    public void Die()
    {

        CameraShakeManager.Instance.Shake(1.75f, 0.15f); //CAMERA SHAKEEE
        StartCoroutine(DeathRumble());

        isDead = true;
        isDashing = false;
        canDash = false;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 0f;
        rb.bodyType = RigidbodyType2D.Kinematic;

    }

    public void Revive()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = defaultGravityScale;
        rb.linearVelocity = Vector2.zero;
        isDead = false;
    }

    private IEnumerator DeathRumble()
    {

        activeGamepad = Gamepad.current;

        if (activeGamepad != null)
        {
            activeGamepad.SetMotorSpeeds(0.35f, 0.75f);
        }

        yield return new WaitForSeconds(0.25f);

        StopRumble();

    }

    public void SetCheckpoint(Transform newCheckpoint)
    {
        currentCheckpoint = newCheckpoint;
    }

    public void OnGamePaused()
    {
        blockGameplayInput = true;

        XInput = 0f;
        YInput = 0f;
        targetSpeed = 0f;

        jumpBufferCounter = 0f;
        dashPressed = false;
        triggerWasPressedLastFrame = false;
    }

    public void OnGameResumed()
    {
        blockGameplayInput = false;

        XInput = 0f;
        YInput = 0f;
        targetSpeed = 0f;

        jumpBufferCounter = 0f;
        dashPressed = false;
        triggerWasPressedLastFrame = false;

        requireJumpReleaseAfterPause = true;
        requireDashReleaseAfterPause = true;
    }

    // GIZMOS --------------------------------------------------------------
    void OnDrawGizmosSelected()
    {
        // MIDDLE GROUND CHECK
        if (groundCheck != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(groundCheck.position, groundCheck.position + Vector3.down * groundCheckDistance);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + (Vector3)groundCheckOffset, transform.position + (Vector3)groundCheckOffset + Vector3.down * groundCheckDistance);
        }

        // LEFT GROUND CHECK
        if (groundCheckLEFT != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(groundCheckLEFT.position, groundCheckLEFT.position + Vector3.down * groundCheckDistance);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + (Vector3)groundCheckOffset, transform.position + (Vector3)groundCheckOffset + Vector3.down * groundCheckDistance);
        }

        // RIGHT GROUND CHECK
        if (groundCheckRIGHT != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(groundCheckRIGHT.position, groundCheckRIGHT.position + Vector3.down * groundCheckDistance);
        }
        else
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawLine(transform.position + (Vector3)groundCheckOffset, transform.position + (Vector3)groundCheckOffset + Vector3.down * groundCheckDistance);
        }
    }
}
