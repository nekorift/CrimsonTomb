using System.Collections;
using UnityEngine;

public class PaladinHammer : MonoBehaviour
{
    // Variables
    [SerializeField] public Vector2 target;
    [SerializeField] public Vector2 origin;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Rigidbody2D body;
    [SerializeField] private BossPaladin paladin;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
        GameObject p = GameObject.FindGameObjectWithTag("Boss");
        paladin = p.GetComponent<BossPaladin>();

        // In case the boss dies before hammer returns, potentially locking the player on the wrong side of the room
        StartCoroutine(DestroyHammer());
    }

    void FixedUpdate()
    {
        body.position = Vector2.MoveTowards(transform.position, target, moveSpeed * Time.fixedDeltaTime);

        if (body.position == target)
            target = origin;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Character player = collision.gameObject.GetComponent<Character>();
            Vector2 direction = (player.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right; // Find direction of knockback
            player.RecieveDamage(paladin.baseDamage, direction);
        }

        if (target == origin)
        {
            if (collision.CompareTag("Boss"))
            {
                paladin.Res();
                Destroy(this.gameObject);
            }
        }
    }

    private IEnumerator DestroyHammer()
    {
        yield return new WaitForSeconds(10f);
        Destroy(this.gameObject);
    }
}
