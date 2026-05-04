using UnityEngine;

public class Character : MonoBehaviour
{
    // Variables
    [SerializeField] protected float maxHealth = 5f;
    [SerializeField] protected float currentHealth;
    [SerializeField] protected float moveSpeed = 5f;
    [SerializeField] protected float maxSpeed = 5f;
    [SerializeField] protected float jumpForce = 10f;
    [SerializeField] protected float baseDamage = 1f;

    // Components
    [SerializeField] protected Rigidbody2D body;
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer spriteRenderer;
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

    protected void RecieveDamage(float damage)
    {
        currentHealth -= damage;
        Debug.Log(gameObject.name + " received " + damage + " damage. Current health: " + currentHealth);
    }
}
