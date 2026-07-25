using UnityEngine;

public class BossPresence1Hitbox : MonoBehaviour
{
    // Variables
    [SerializeField] private BossPresence1 presence;

    private void Start()
    {
        presence = GetComponentInParent<BossPresence1>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Character player = collision.gameObject.GetComponent<Character>();
            Vector2 direction = (player.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right; // Find direction of knockback
            player.RecieveDamage(presence.baseDamage, direction);
        }
    }
}
