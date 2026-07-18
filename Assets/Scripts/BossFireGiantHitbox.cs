using UnityEngine;

public class BossFireGiantHitbox : MonoBehaviour
{
    // Variables
    [SerializeField] private BossFireGiant giant;

    private void Start()
    {
        giant = GetComponentInParent<BossFireGiant>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Character player = collision.gameObject.GetComponent<Character>();
            Vector2 direction = (player.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right; // Find direction of knockback
            player.RecieveDamage(giant.baseDamage, direction);
        }
    }
}
