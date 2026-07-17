using UnityEngine;

public class PlayerAttackHitboxDown : MonoBehaviour
{
    [SerializeField] private Player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            Character enemy = collision.gameObject.GetComponent<Character>();
            Character self = GetComponentInParent<Character>();
            Vector2 direction = (enemy.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right; // Find direction of knockback
            enemy.RecieveDamage(self.baseDamage, direction);

            if (player != null)
            {
                player.body.linearVelocity = new Vector2(player.body.linearVelocity.x, 0); // Reset vertical velocity to prevent stacking forces
                player.body.AddForce(new Vector2(0, 5), ForceMode2D.Impulse); // Apply upward force to the player
            }
        }
    }
}
