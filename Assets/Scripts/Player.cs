using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    // Variables
    public bool canJump;
    public float currentJumps = 0;
    public bool facingRight = true; // true for right, false for left
    public bool isOnGround = false;
    public LayerMask groundLayer;
    // Abilities
    public bool hasDoubleJump = false;
    public float maxJumps = 1;
    public bool hasDash = false;
    public bool canDash = false;
    public bool isDashing = false;
    public float dashVelocity = 5f;
    public float dashTime = 0.2f;
    public bool hasSprint = false;
    public bool hasWallJump = false;

    // Components
    [SerializeField] Camera cam;
    PlayerInput input;

    // Inputs
    Vector2 movementInput;

    protected override void Start()
    {
        base.Start();
        input = GetComponent<PlayerInput>();
        cam = Camera.main;
    }

    void Update()
    {
        // Camera Follow
        cam.transform.position = new Vector3(transform.position.x, transform.position.y, cam.transform.position.z);

        // Movement
        //body.linearVelocity = new Vector2(movementInput.x * moveSpeed, body.linearVelocity.y);
        body.AddForce(new Vector2(movementInput.x, 0) * moveSpeed, ForceMode2D.Force);

        if (movementInput.x < 0.5 && movementInput.x > -0.5f && !isDashing)
            body.linearVelocity = new Vector2(0, body.linearVelocity.y);

        if (body.linearVelocity.x > maxSpeed && !isDashing)
            body.linearVelocity = new Vector2(maxSpeed, body.linearVelocity.y);
        else if (body.linearVelocity.x < -maxSpeed && !isDashing)
            body.linearVelocity = new Vector2(-maxSpeed, body.linearVelocity.y);

        CheckIsOnGround();
    }

    private void CheckIsOnGround()
    {
        float checkRadius = 0.2f;
        Vector2 checkPosition = new Vector2(transform.position.x, transform.position.y - 0.7f - checkRadius); // Checked with OnDrawGizmosSelected to ensure the position is correct

        isOnGround = Physics2D.OverlapCircle(checkPosition, checkRadius, groundLayer);

        if (isOnGround)
        {
            currentJumps = 0;
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
        if (currentJumps < maxJumps)
            canJump = true;
        else
            canJump = false;

        if (input.isPressed && canJump)
        {
            body.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            currentJumps++;
        }
    }

    public void OnDash(InputValue input)
    {
        if (input.isPressed && hasDash && canDash && !isDashing)
        {
            /*
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

        Debug.Log("Dash activated! Facing right: " + facingRight + ", Can dash: " + canDash + ", Has dash: " + hasDash);
        // Dash
        canDash = false;
        isDashing = true;
        float originalGravity = body.gravityScale; // Ensure to store the original gravity scale to restore it later
        body.gravityScale = 0f;
        body.linearVelocity = dashDirection * dashVelocity;
        yield return new WaitForSeconds(dashTime);
        body.gravityScale = originalGravity;
        isDashing = false;
    }
}
