using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    // Variables
    [SerializeField] protected float maxHealth = 1f;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float maxSpeed = 5f;
    [SerializeField] protected float jumpForce = 10f;
    [SerializeField] public float baseDamage = 1f;
    [SerializeField] protected bool activeIframes = false;
    [SerializeField] protected float iframeLength = 0.5f;

    [SerializeField] protected bool canJump;
    [SerializeField] protected LayerMask groundLayer;

    [SerializeField] protected bool facingRight = true; // true for right, false for left

    // Components
    protected Rigidbody2D body;
    [SerializeField] protected Animator animator;
    protected SpriteRenderer spriteRenderer;
    [SerializeField] protected Collider2D col;

    protected virtual void Start()
    { 
        // Get components
        body = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        // Initialize health
        currentHealth = maxHealth;
    }

    public void RecieveDamage(float damage, Vector2 direction)
    {
        if (!activeIframes)
        {
            currentHealth -= damage;
            StartCoroutine(Iframe());

            if (gameObject.tag == "Player")
            {
                body.linearVelocity = Vector2.zero; // Reset velocity to ensure consistent knockback regardless of current movement
                //body.linearVelocity += new Vector2(direction ? -15f : 15f, 5f);
                body.AddForce(new Vector2(direction.x * 5f, 5f), ForceMode2D.Impulse);
            }

            Debug.Log(gameObject.name + " received " + damage + " damage. Current health: " + currentHealth);

            if (currentHealth <= 0)
            {
                // ADD DEATH LOGIC HERE
            }
        }
        else
            Debug.Log(gameObject.name + " has i-frames active.");
    }

    private IEnumerator Iframe()
    {
        activeIframes = true;
        yield return new WaitForSeconds(iframeLength);
        activeIframes = false;
    }

    protected GameObject FindPlayer()
    {
        GameObject player;
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found in the scene. Please ensure there is a GameObject with the tag 'Player'.");
            return null;
        }
        else
            return player;
    }

    protected void TurnToPlayer(GameObject player)
    {
        if (player.transform.position.x > transform.position.x)
            facingRight = true;
        else
            facingRight = false;
    }

    protected bool IsOnGround()
    {
        float checkRadius = 0.2f;
        //Vector2 checkPosition = new Vector2(transform.position.x, transform.position.y - 0.7f - checkRadius); // Didn't allow different heights
        Vector2 checkPosition = new Vector2(transform.position.x, transform.position.y - (col.bounds.size.y / 2));

        return Physics2D.OverlapCircle(checkPosition, checkRadius, groundLayer);
    }

    protected bool IsTouchingWall()
    {
        float checkRadius = 0.2f;
        Vector2 checkPosition = new Vector2(transform.position.x + (facingRight ? (col.bounds.size.x / 2) : -(col.bounds.size.x / 2)), transform.position.y);

        return Physics2D.OverlapCircle(checkPosition, checkRadius, groundLayer);
    }

    protected bool IsBackTouchingWall()
    {
        float checkRadius = 0.2f;
        Vector2 checkPosition = new Vector2(transform.position.x + (facingRight ? -(col.bounds.size.x / 2) : (col.bounds.size.x / 2)), transform.position.y);

        return Physics2D.OverlapCircle(checkPosition, checkRadius, groundLayer);
    }

    protected void OnDrawGizmosSelected()
    {
        // Ground check gizmo
        Gizmos.color = Color.red;
        Vector2 groundCheckPosition = new Vector2(transform.position.x, transform.position.y - (col.bounds.size.y / 2));
        Gizmos.DrawWireSphere(groundCheckPosition, 0.2f);

        // Wall check gizmo
        Gizmos.color = Color.blue;
        Vector2 wallCheckPosition = new Vector2(transform.position.x + (facingRight ? (col.bounds.size.x / 2) : -(col.bounds.size.x / 2)), transform.position.y);
        Gizmos.DrawWireSphere(wallCheckPosition, 0.2f);

        // Back wall check gizmo
        Gizmos.color = Color.yellow;
        Vector2 backWallCheckPosition = new Vector2(transform.position.x + (facingRight ? -(col.bounds.size.x / 2) : (col.bounds.size.x / 2)), transform.position.y);
        Gizmos.DrawWireSphere(backWallCheckPosition, 0.2f);
    }
}
