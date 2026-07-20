using UnityEngine;

public class BossRatHitbox : MonoBehaviour
{
    // Variables
    [SerializeField] private BossRat rat;

    private void Start()
    {
        rat = GetComponentInParent<BossRat>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Character player = collision.gameObject.GetComponent<Character>();
            Vector2 direction = (player.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right; // Find direction of knockback
            player.RecieveDamage(rat.baseDamage, direction);
        }
    }
}
