using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    // Variables
    [SerializeField] private float currentJumps = 0;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private float attackLength = 0.3f;

    // Abilities Variables
    public bool hasDoubleJump = false;
    [SerializeField] private float maxJumps;

    public bool hasDash = false;
    [SerializeField] private bool canDash = false;
    [SerializeField] private bool isDashing = false;
    [SerializeField] private float dashVelocity = 5f;
    [SerializeField] private float dashTime = 0.2f;

    public bool hasSprint = false;
    [SerializeField] private bool isSprinting = false;
    [SerializeField] private float sprintSpeed = 10f;

    public bool hasWallSlide = false;

    // Components
    private Camera cam;
    private PlayerInput input;
    [SerializeField] private GameObject rightAttack;
    [SerializeField] private GameObject leftAttack;

    // Input
    private Vector2 movementInput;

    protected override void Start()
    {
        base.Start();
        input = GetComponent<PlayerInput>();
        cam = Camera.main;
    }

    void Update()
    {
        // Camera
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);

        // Movement
        //body.linearVelocity = new Vector2(movementInput.x * moveSpeed, body.linearVelocity.y);
        if (!isDashing && !activeIframes) // Ensure only dash can override the player's velocity, otherwise the player will be able to move during the dash which is not intended
            body.AddForce(new Vector2(movementInput.x, 0) * moveSpeed, ForceMode2D.Force);

        if (!isSprinting && !isDashing)
        {
            if (body.linearVelocity.x > maxSpeed)
                body.linearVelocity = new Vector2(maxSpeed, body.linearVelocity.y);
            else if (body.linearVelocity.x < -maxSpeed)
                body.linearVelocity = new Vector2(-maxSpeed, body.linearVelocity.y);
        }
        else if (isSprinting && !isDashing)
        {
            if (body.linearVelocity.x > sprintSpeed)
                body.linearVelocity = new Vector2(sprintSpeed, body.linearVelocity.y);
            else if (body.linearVelocity.x < -sprintSpeed)
                body.linearVelocity = new Vector2(-sprintSpeed, body.linearVelocity.y);
        }

        if (movementInput.x < 0.5 && movementInput.x > -0.5f && !isDashing && !activeIframes && IsOnGround())
        {
            body.linearVelocity = new Vector2(0, body.linearVelocity.y);
            StartCoroutine(StopSprinting()); // Stop sprinting after a short delay to allow for switching sides without immediately stopping sprinting
        }

        if (hasWallSlide)
        {
            if (IsBackTouchingWall())
                currentJumps = 0; // Reset jumps when wall sliding to allow for infinite wall jumps regardless of whether the player has double jump or not

            if (IsBackTouchingWall() && !IsOnGround() && movementInput.x != 0)
                body.linearVelocity = new Vector2(body.linearVelocity.x, Mathf.Clamp(body.linearVelocity.y, -2f, float.MaxValue)); // Limit the player's falling speed while wall sliding
            else if (IsTouchingWall() && !IsOnGround() && movementInput.x != 0)
                facingRight = !facingRight; // Flip the player's facing direction if they are touching a wall and not on the ground, to allow for wall sliding in both directions
        }

        if (IsOnGround())
        {
            currentJumps = 0;
            //activeIframes = false; // Remove i-frames if the player touches the ground before iframes run out
            canJump = true;
            canDash = true;
        }
    }

    public void OnMove(InputValue input)
    {
        movementInput = input.Get<Vector2>();

        if (movementInput.x > 0)
            facingRight = true;
        else if (movementInput.x < 0)
            facingRight = false;
    }

    public void OnJump(InputValue input)
    {
        // Set how many jumps the player can make
        if (hasDoubleJump)
            maxJumps = 2;
        else
            maxJumps = 1;

        // Check if the player can jump
        if (currentJumps < maxJumps && !isDashing)
            canJump = true;
        else
            canJump = false;

        if (input.isPressed && canJump && IsOnGround())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, 0); // Reset vertical velocity to ensure consistent jump height
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            currentJumps++;
        }
        else if (input.isPressed && canJump && hasWallSlide && IsBackTouchingWall() && !IsOnGround())
        {
            body.linearVelocity = Vector2.zero; // Reset velocity to ensure consistent jump height
            body.AddForce(new Vector2(-movementInput.x * jumpForce * 2, jumpForce), ForceMode2D.Impulse); // Jump in the opposite direction of the wall
            currentJumps++;
        }
        else if (input.isPressed && canJump && !IsOnGround() && hasDoubleJump)
        {
            if (currentJumps == 0)
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, 0); // Reset vertical velocity to ensure consistent jump height
                body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                currentJumps++;
                currentJumps++; // Incrementing twice to skip the first jump since the player is already in the air
            }
            else if (currentJumps == 1)
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, 0); // Reset vertical velocity to ensure consistent jump height
                body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                currentJumps++;
            }
        }
    }

    public void OnAttack(InputValue input)
    {
        if (input.isPressed && !isDashing && !activeIframes && !isAttacking)
        {
            if (facingRight)
                StartCoroutine(Attack(rightAttack));
            else
                StartCoroutine(Attack(leftAttack));
        }
    }

    private IEnumerator Attack(GameObject attack)
    {
        Debug.Log("Attack activated! Facing right: " + facingRight);
        Collider2D attackCollider = attack.GetComponent<Collider2D>();  

        isAttacking = true;
        attackCollider.enabled = true;
        yield return new WaitForSeconds(attackLength);
        isAttacking = false;
        attackCollider.enabled = false;
    }

    public void OnSprint(InputValue input)
    {
        if (input.isPressed && hasSprint && IsOnGround())
            isSprinting = !isSprinting; // Toggle sprint
    }

    private IEnumerator StopSprinting()
    {
        yield return new WaitForSeconds(0.5f); // Delay before stopping sprinting to allow for switching sides without immediately stopping sprinting

        if (movementInput.x < 0.5 && movementInput.x > -0.5f && !isDashing)
            isSprinting = false;
    }

    public void OnDash(InputValue input)
    {
        if (input.isPressed && hasDash && canDash && !isDashing)
        {
            /* Didn't work without coroutine, as it turns off gravity during the dash which needs to be turned back on after a short time, otherwise the player will just fly indefinitely after dashing
            canDash = false;

            //Vector2 dashDirection = new Vector2(movementInput.x, 0).normalized;
            Vector2 dashDirection = Vector2.zero;
            if (facingRight)
                dashDirection = Vector2.right;
            else
                dashDirection = Vector2.left;

            body.AddForce(dashDirection * dashVelocity, ForceMode2D.Impulse);
            */

            StartCoroutine(Dash());
        }
    }

    private IEnumerator Dash()
    {
        // Get direction
        Vector2 dashDirection = Vector2.zero;
        if (facingRight)
            dashDirection = Vector2.right;
        else
            dashDirection = Vector2.left;

        // Dash
        //Debug.Log("Dash activated! Facing right: " + facingRight + ", Can dash: " + canDash + ", Has dash: " + hasDash);
        canDash = false;
        isDashing = true;
        float originalGravity = body.gravityScale; // Ensure to store the original gravity scale to restore it later
        body.gravityScale = 0f;
        body.linearVelocity = dashDirection * dashVelocity;

        yield return new WaitForSeconds(dashTime);

        //Debug.Log("Dash successful!");
        body.gravityScale = originalGravity;
        isDashing = false;
    }
}
