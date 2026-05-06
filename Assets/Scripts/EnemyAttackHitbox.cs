using UnityEngine;

public class EnemyAttackHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Character player = collision.gameObject.GetComponent<Character>();
            Character self = GetComponentInParent<Character>();
            Vector2 direction = (player.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right; // Find direction of knockback
            player.RecieveDamage(self.baseDamage, direction);
        }
    }
}
