using UnityEngine;

public class BossBatHitbox : MonoBehaviour
{
    // Variables
    [SerializeField] private BossBat bat;
    [SerializeField] private int count;

    private void Start()
    {
        bat = GetComponentInParent<BossBat>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Character player = collision.gameObject.GetComponent<Character>();
            Vector2 direction = (player.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right; // Find direction of knockback
            player.RecieveDamage(bat.baseDamage, direction);
            bat.isAttacking = false;
            count = 0;
        }

        if (collision.gameObject.CompareTag("PlayerAttack"))
            count++;

        if (count >= 2)
        {
            bat.isAttacking = false;
            count = 0;
        }
    }
}
