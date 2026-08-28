using UnityEngine;

public class BossPresence2Hitbox : MonoBehaviour
{
    // Variables
    [SerializeField] private BossPresence2 presence;

    private void Start()
    {
        presence = GetComponentInParent<BossPresence2>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Character player = collision.gameObject.GetComponent<Character>();
            Vector2 direction = (player.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right; // Find direction of knockback
            player.RecieveDamage(presence.baseDamage, direction);
        }
    }
}
