using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;

/*
------------------- NOTES -------------------

Can check jump animation also with y velocity, but need 2 animations - jump up and fall down.

MAYBE Need to add more Colliders if / else if / else if (closer to player center overrides to snap bigger distance)
*/

public class PlayerController : MonoBehaviour
{
    public float speed = 6f;
    public float jumpForce = 6f;

    [SerializeField] private float maxRunSpeed = 6f;
    [SerializeField] private float runAcceleration = 20f;
    [SerializeField] private float runDeceleration = 15f;

    private float targetSpeed;
    private float accelRate;
    //private bool isGroundedEDGE;

    public Transform groundCheck;
    public float groundCheckDistance = .12f;
    public Vector2 groundCheckOffset = new Vector2(0f, -.5f);

    public Transform groundCheckLEFT;
    public Transform groundCheckRIGHT;
    public LayerMask groundLayer;

    private Rigidbody2D rb;
    private float XInput;
    private float YInput;
    private bool isGrounded;

    //this is only for old flip
    //private SpriteRenderer spriteRenderer;

    //new flip
    public bool isFacingRight = true;

    private Animator anim;

    [SerializeField] private float coyoteTime = 0.2f;
    private float coyoteTimeCounter;
    [SerializeField] private float jumpBufferTime = 0.2f;
    private float jumpBufferCounter;

    //dash
    [SerializeField] private float dashingVelocity = 14f;
    [SerializeField] private float dashingTime = 0.5f;
    private Vector2 dashingDir;
    public bool isDashing; //important to be public for TilemapSwitcheroo to work
    private bool canDash = true;

    public TrailRenderer tr;
    private float defaultGravityScale;
    [SerializeField] private float maxFallSpeed = 20f;
    [SerializeField] private float jumpHangTimeThreshold = 0.2f;
    [SerializeField] private float jumpHangGravityMult = 0.5f;
    [SerializeField] private float jumpHangMaxSpeedMult = 0.8f;
    [SerializeField] private float jumpHangAccelMult = 0.8f;

    //private bool isWallJumping = false;
    private bool isJumpFalling = false;

    //wall jumping
    /*    private bool isWallJumping;
        private float wallJumpingDir;
        [SerializeField] private float wallJumpingTime = 0.2f;
        private float wallJumpingCounter;
        [SerializeField] private float wallJumpingDuration = 0.4f;
        [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);

        //wall sliding
        private bool isWallSliding;
        [SerializeField] private float wallSlidingSpeed = 8f;
        [SerializeField] private Transform RightWallCheck;
        [SerializeField] private LayerMask wallLayer; */

    public TilemapSwitch tilemapswitch;

    public ParticleSystem smokeFX;

    //dash controller input fix var
    float dashTrigger;
    //float dashTrigger2;
    bool dashPressed;
    bool triggerWasPressedLastFrame;

    //camera follow
    //private CameraFollowObject cameraFollowObject;
    [SerializeField] private GameObject cameraFollowGO;

    //Controller
    private Gamepad activeGamepad;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        tr = GetComponent<TrailRenderer>();

        defaultGravityScale = rb.gravityScale;

        Vector3 rotator = transform.eulerAngles;
        rotator.y = isFacingRight ? 0f : -180f;
        transform.eulerAngles = rotator;

        //cameraFollowObject = cameraFollowGO.GetComponent<CameraFollowObject>();
    }

    // Update is called once per frame
    void Update()
    {

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

        // DASH -----------------------------------------------------------------

        //if (!PauseMenuManager.isPaused)
        //{
        if (dashInput && canDash)
        {
            isDashing = true;
            canDash = false;
            tr.emitting = true;
            dashingDir = new Vector2(XInput, YInput);

            //fix for dpad bug with dash
            if (Mathf.Abs(dpadX) > 0.1f)
            {
                dashingDir = new Vector2(dpadX, 0f);
            }
            else if (Mathf.Abs(dpadY) > 0.1f)
            {
                dashingDir = new Vector2(0f, dpadY);
            }
            else
            {
                dashingDir = new Vector2(XInput, YInput);
            }

            //controller rumble
            activeGamepad = Gamepad.current;

            if (activeGamepad != null)
            {
                activeGamepad.SetMotorSpeeds(0.25f, 0.65f);
            }

            //sound effect
            if (AudioManager.Instance != null && AudioManager.Instance.dashSFX != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.dashSFX);
            }

            smokeFX.Play();
            tilemapswitch.TilemapSwitcheroo(); //TILEMAP SWITCHEROOOO
            CameraShakeManager.Instance.Shake(1.5f, 0.15f); //CAMERA SHAKEEE

            if (dashingDir == Vector2.zero)
            {
                float facing = isFacingRight ? 1f : -1f;
                dashingDir = new Vector2(facing, 0f);
            }
            StartCoroutine(StopDashing());
        }
        //}

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

        //if (!PauseMenuManager.isPaused)
        //{
        targetSpeed = XInput * maxRunSpeed;

        if (Mathf.Abs(targetSpeed) > 0.01f)
            accelRate = runAcceleration;
        else
            accelRate = runDeceleration;
        //}

        // COYOTE ---------------------------------------------------------------

        if (isGrounded /*&& isGroundedEDGE*/)
        {
            coyoteTimeCounter = coyoteTime;
            canDash = true;
        }
        else coyoteTimeCounter -= Time.deltaTime;

        // JUMP BUFFER ----------------------------------------------------------
        //if (!PauseMenuManager.isPaused)
        //{
        if (Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        if (jumpBufferCounter > 0f && coyoteTimeCounter > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);

            if (AudioManager.Instance != null && AudioManager.Instance.jumpSFX != null)
            {
                AudioManager.Instance.PlaySFX(AudioManager.Instance.jumpSFX);
            }

            jumpBufferCounter = 0f;
        }
        //}

        // HIGHER JUMP ON HOLD ------------------------------------------------------

        //if (!PauseMenuManager.isPaused)
        //{
        if (Input.GetButtonUp("Jump") && rb.linearVelocity.y > 0f)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, rb.linearVelocity.y * 0.4f); //change this 0.0f to lower the smallest jump possible

            coyoteTimeCounter = 0f;
        }
        //}

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
            isJumpFalling = true;
        }
        else
            isJumpFalling = false;

        bool inAir = !isGrounded /*&& !isGroundedEDGE*/;
        bool nearY0 = Mathf.Abs(rb.linearVelocity.y) < jumpHangTimeThreshold;
        bool inJumping = inAir && nearY0;

        // HOLD MID-AIR ----------------------------------------------------------

        if (inJumping)
        {
            desiredGravity = defaultGravityScale * jumpHangGravityMult;
            accelRate *= jumpHangAccelMult;
            targetSpeed *= jumpHangMaxSpeedMult;
        }

        rb.gravityScale = desiredGravity;

        //WallSlide();
        //WallJump();
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

            //turn the camera follow object
            //cameraFollowObject.CallTurn();
        }
        else
        {
            Vector3 rotator = new Vector3(transform.rotation.x, 0f, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotator);
            isFacingRight = !isFacingRight;

            //turn the camera follow object
            //cameraFollowObject.CallTurn();
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
        if (isDashing) return;

        float maxSpeedChange = accelRate * Time.fixedDeltaTime;
        float newX = Mathf.MoveTowards(rb.linearVelocity.x, targetSpeed, maxSpeedChange);

        //if (!isWallJumping)
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

    // WALL SLIDE -------------------------------------------------------------
    /*
    private bool isWalled()
    {
        return Physics2D.OverlapCircle(RightWallCheck.position, 0.2f, wallLayer);
    }

    private bool WallSlide()
    {
        if (isWalled() && !isGrounded && !isGroundedEDGE && XInput != 0f)
        {
            isWallSliding = true;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Clamp(rb.linearVelocity.y, -wallSlidingSpeed, float.MaxValue));
            return true;
        }
        else
        {
            isWallSliding = false;
            return false;
        }
    }

// WALL JUMP -------------------------------------------------------------
    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDir = spriteRenderer.flipX ? 1f : -1f;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f) here put wallJumping counter <= 0 ??
        {
            isWallJumping = true;
            rb.linearVelocity = new Vector2(wallJumpingDir * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (wallJumpingDir > 0)
                spriteRenderer.flipX = false;
            else
                spriteRenderer.flipX = true;

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }
    */

    // DASH ----------------------------------------------------------------
    private IEnumerator StopDashing()
    {
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
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

        // LEFT OUTER LEDGE CHECK
        // if (left_outer != null)
        // {
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawLine(left_outer.position, left_outer.position + Vector3.up * groundCheckDistance);
        // }
        // else
        // {
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawLine(transform.position + (Vector3)groundCheckOffset, transform.position + (Vector3)groundCheckOffset + Vector3.up * groundCheckDistance);
        // }

        // // LEFT INNER LEDGE CHECK
        // if (left_inner != null)
        // {
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawLine(left_inner.position, left_inner.position + Vector3.up * groundCheckDistance);
        // }
        // else
        // {
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawLine(transform.position + (Vector3)groundCheckOffset, transform.position + (Vector3)groundCheckOffset + Vector3.up * groundCheckDistance);
        // }

        // // RIGHT OUTER LEDGE CHECK
        // if (right_outer != null)
        // {
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawLine(right_outer.position, right_outer.position + Vector3.up * groundCheckDistance);
        // }
        // else
        // {
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawLine(transform.position + (Vector3)groundCheckOffset, transform.position + (Vector3)groundCheckOffset + Vector3.up * groundCheckDistance);
        // }

        // // RIGHT INNER LEDGE CHECK
        // if (right_inner != null)
        // {
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawLine(right_inner.position, right_inner.position + Vector3.up * groundCheckDistance);
        // }
        // else
        // {
        //     Gizmos.color = Color.yellow;
        //     Gizmos.DrawLine(transform.position + (Vector3)groundCheckOffset, transform.position + (Vector3)groundCheckOffset + Vector3.up * groundCheckDistance);
        // }
    }
}
