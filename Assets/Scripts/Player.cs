using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : Character
{
    // Variables
    [SerializeField] private float currentJumps = 0;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private float attackLength = 0.3f;
    [SerializeField] public Vector2 spawnLocation;
    [SerializeField] private bool canInteract = false;
    [SerializeField] private GameObject interactableObject = null;
    private Coroutine attackRoutine = null;

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
    private PlayerInput input;
    [SerializeField] private GameObject rightAttack;
    [SerializeField] private GameObject leftAttack;
    [SerializeField] private GameObject downAttack;
    [SerializeField] private GameObject upAttack;

    // UI Components
    [SerializeField] public GameObject[] UiHp;
    [SerializeField] public Sprite[] hearts;
    [SerializeField] public GameObject blackScreen;
    [SerializeField] public GameObject pauseMenu;
    [SerializeField] public bool isPaused;

    // Input
    private Vector2 movementInput;

    // Sound Effects
    [SerializeField] private AudioSource attackSfx;
    [SerializeField] private AudioSource dashSfx;
    [SerializeField] public AudioSource hitSfx;

    protected override void Start()
    {
        DontDestroyOnLoad(this);
        base.Start();
        transform.position = spawnLocation;
        input = GetComponent<PlayerInput>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            movementInput = Vector2.zero; // Prevent movement input when dead
        }

        // Movement
        if (!isAttacking)
        {
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

            if (movementInput.x < 0.1 && movementInput.x > -0.1f && !isDashing && !activeIframes && IsOnGround())
            {
                body.linearVelocity = new Vector2(0, body.linearVelocity.y);
                StartCoroutine(StopSprinting()); // Stop sprinting after a short delay to allow for switching sides without immediately stopping sprinting
            }
        }

        if (isAttacking && IsOnGround())
        {
            body.linearVelocity = new Vector2(0, body.linearVelocity.y); // Prevent movement input during attack to ensure the player can't move during the attack animation, which is not intended
        }

        if (body.linearVelocity.x > 0.1f)
            facingRight = true;
        else if (body.linearVelocity.x < -0.1f)
            facingRight = false;

        if (hasWallSlide)
        {
            if (IsBackTouchingWall() && !IsOnGround() && movementInput.x != 0)
                body.linearVelocity = new Vector2(body.linearVelocity.x, Mathf.Clamp(body.linearVelocity.y, -2f, float.MaxValue)); // Limit the player's falling speed while wall sliding
            else if (IsTouchingWall() && !IsOnGround() && movementInput.x != 0)
                facingRight = !facingRight; // Flip the player's facing direction if they are touching a wall and not on the ground, to allow for wall sliding in both directions
        }

        if (IsOnGround() || (hasWallSlide && IsBackTouchingWall()))
        {
            currentJumps = 0;
            //activeIframes = false; // Remove i-frames if the player touches the ground before iframes run out
            canJump = true;
            canDash = true;
        }

        // Sprites and animations
        if(facingRight)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        if (currentHealth <= 0)
            animator.Play("PlayerDeath");
        else if (activeIframes && !IsOnGround())
            animator.Play("PlayerDamageAir");
        else if (activeIframes && IsOnGround())
            animator.Play("PlayerDamageGround");
        else if (isAttacking)
        {
            if (attackRoutine == null)
            {
                if (movementInput.x < 0)
                {
                    if (movementInput.y <= movementInput.x && !IsOnGround())
                    {
                        attackRoutine = StartCoroutine(Attack(downAttack));
                        animator.Play("PlayerAttackDown");
                    }
                    else if (movementInput.y >= -movementInput.x)
                    {
                        attackRoutine = StartCoroutine(Attack(upAttack));
                        animator.Play("PlayerAttackUp");
                    }
                    else
                    {
                        attackRoutine = StartCoroutine(Attack(leftAttack));
                        animator.Play("PlayerAttack");
                    }
                }
                else if (movementInput.x > 0)
                {
                    if (movementInput.y <= -movementInput.x && !IsOnGround())
                    {
                        attackRoutine = StartCoroutine(Attack(downAttack));
                        animator.Play("PlayerAttackDown");
                    }
                    else if (movementInput.y >= movementInput.x)
                    {
                        attackRoutine = StartCoroutine(Attack(upAttack));
                        animator.Play("PlayerAttackUp");
                    }
                    else
                    {
                        attackRoutine = StartCoroutine(Attack(rightAttack));
                        animator.Play("PlayerAttack");
                    }
                }
                else
                {
                    if (movementInput.y < 0 && !IsOnGround())
                    {
                        attackRoutine = StartCoroutine(Attack(downAttack));
                        animator.Play("PlayerAttackDown");
                    }
                    else if (movementInput.y > 0)
                    {
                        attackRoutine = StartCoroutine(Attack(upAttack));
                        animator.Play("PlayerAttackUp");
                    }
                    else
                    {
                        if (facingRight)
                            attackRoutine = StartCoroutine(Attack(rightAttack));
                        else
                            attackRoutine = StartCoroutine(Attack(leftAttack));

                        animator.Play("PlayerAttack");
                    }
                }
            }
        }
        else if (isDashing)
            animator.Play("PlayerDash");
        else if (hasWallSlide && IsBackTouchingWall() && !IsOnGround() && movementInput.x != 0)
            animator.Play("PlayerWallSlide");
        else if (!IsOnGround())
            animator.Play("PlayerJump");
        else if (movementInput.x != 0 && IsOnGround() && !isSprinting)
            animator.Play("PlayerWalk");
        else if (movementInput.x != 0 && IsOnGround() && isSprinting)
            animator.Play("PlayerSprint");
        else if (movementInput.x == 0 && IsOnGround())
            animator.Play("PlayerIdle");

        for (int i = 0; i < maxHealth; i++)
        {
            if (i < currentHealth)
            {
                UiHp[i].GetComponent<Image>().sprite = hearts[1];
            }
            else
            {
                UiHp[i].GetComponent<Image>().sprite = hearts[0];
            }
        }
    }

    public void OnMove(InputValue input)
    {
        movementInput = input.Get<Vector2>();
    }

    public void OnJump(InputValue input)
    {
        Debug.Log("Jump input: " + input.isPressed);

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

        if (!input.isPressed)
        {
            if (body.linearVelocity.y > 0)
            {
                body.linearVelocity = new Vector2(body.linearVelocity.x, 0); // Reset vertical velocity when jump button is released early
            }
        }
    }

    public void OnAttack(InputValue input)
    {
        if (input.isPressed && !isDashing && !activeIframes && !isAttacking && !isPaused)
        {
            //if (facingRight)
            //    StartCoroutine(Attack(rightAttack));
            //else
            //    StartCoroutine(Attack(leftAttack));
            isAttacking = true;
        }
    }

    private IEnumerator Attack(GameObject attack)
    {
        Debug.Log("Attack activated! Facing right: " + facingRight);
        Collider2D attackCollider = attack.GetComponent<Collider2D>();  

        attackSfx.Play();

        //isAttacking = true;
        attackCollider.enabled = true;
        yield return new WaitForSeconds(attackLength);
        isAttacking = false;
        attackCollider.enabled = false;
        attackRoutine = null;
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

        dashSfx.Play();

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

    public void OnInteract(InputValue input)
    {
        if (input.isPressed && canInteract)
        {
            if (interactableObject.CompareTag("Seal"))
            {
                if (SceneManager.GetActiveScene().name == "Spires5")
                {
                    hasDoubleJump = true;
                }
                else if (SceneManager.GetActiveScene().name == "Causeway5")
                {
                    hasSprint = true;
                }
                else if (SceneManager.GetActiveScene().name == "Canals5")
                {
                    hasDash = true;
                }
                else if (SceneManager.GetActiveScene().name == "Forge5")
                {
                    hasWallSlide = true;
                }

                interactableObject.GetComponent<Seal>().BreakSeal();
            }
            else if (interactableObject.CompareTag("Door"))
            {
                if (interactableObject.GetComponent<Door>().open)
                {
                    interactableObject.GetComponent<Door>().LoadScene();
                }
            }
            else
                Debug.Log("Player interacted with " + interactableObject.name + " but it has no interaction logic.");
        }
    }

    public void OnPause(InputValue input)
    {
        if (input.isPressed)
        {
            PauseGame();
        }
    }

    public void PauseGame()
    {
        isPaused = !isPaused;
        pauseMenu.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseMenu.SetActive(true);
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.SetActive(false);
        }
    }

    public void MainMenu()
    {
        GameManager gm = FindAnyObjectByType<GameManager>();
        gm.ReturnToMenu();
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Seal")
        {
            canInteract = true;
            interactableObject = collision.gameObject;
            Debug.Log("Player can interact with " + collision.gameObject.name);

            // Add UI prompt
        }
        else if (collision.gameObject.tag == "Door")
        {
            canInteract = true;
            interactableObject = collision.gameObject;
            Debug.Log("Player can interact with " + collision.gameObject.name);

            // Add UI prompt
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Seal")
        {
            canInteract = false;
            interactableObject = null;
            Debug.Log("Player can no longer interact with " + collision.gameObject.name);

            // Remove UI prompt
        }
        else if (collision.gameObject.tag == "Door")
        {
            canInteract = false;
            interactableObject = null;
            Debug.Log("Player can no longer interact with " + collision.gameObject.name);

            // Remove UI prompt
        }
    }
}
