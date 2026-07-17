using UnityEngine;

public class PlayerAttackHitbox : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy") || collision.CompareTag("Boss"))
        {
            Character enemy = collision.gameObject.GetComponent<Character>();
            Character self = GetComponentInParent<Character>();
            Vector2 direction = (enemy.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right; // Find direction of knockback
            enemy.RecieveDamage(self.baseDamage, direction);
        }
    }
}
