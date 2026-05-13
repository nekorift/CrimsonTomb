using System.Collections;
using UnityEngine;

public class EnemyJumper : Character
{
    // Variables
    [SerializeField] private GameObject player;
    [SerializeField] private Sprite[] sprites;

    protected override void Start()
    {
        base.Start();
        player = FindPlayer();
        TurnToPlayer(player);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //int layerGround = LayerMask.NameToLayer("Ground");

        if (collision.gameObject.tag == "Player")
        {
            Character player = collision.gameObject.GetComponent<Character>();
            Vector2 direction = (player.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right; // Find direction of knockback
            player.RecieveDamage(baseDamage, direction);
        }
        else if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Debug.Log(gameObject.name + " collided with the ground");
            body.linearVelocity = new Vector2(0, body.linearVelocity.y); // Reset horizontal velocity to ensure enemy doesnt slide across the floor
            TurnToPlayer(player);
            spriteRenderer.sprite = sprites[0];
            StartCoroutine(Jump());
        }

        if (facingRight)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;
    }

    private IEnumerator Jump()
    {
        yield return new WaitForSeconds(1.5f); // Delay the enemy from jumping instantly
        spriteRenderer.sprite = sprites[1];
        body.AddForce(new Vector2(facingRight ? moveSpeed : -moveSpeed, jumpForce), ForceMode2D.Impulse);
    }
}
