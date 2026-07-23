using System.Collections;
using UnityEngine;

public class BloodArrow : MonoBehaviour
{
    // Variables
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float destroyTime = 5f;
    [SerializeField] private bool isMoving = false;

    // Components
    [SerializeField] private Collider2D col;
    [SerializeField] private Rigidbody2D body;

    private void Start()
    {
        col = GetComponent<Collider2D>();
        body = GetComponent<Rigidbody2D>();
        col.enabled = false;
    }

    void FixedUpdate()
    {
        if (isMoving)
            body.position += (Vector2)transform.up * moveSpeed * Time.fixedDeltaTime;
    }

    public void StartMoving()
    {
        col.enabled = true;
        isMoving = true;
        StartCoroutine(DestroyArrow());
    }

    IEnumerator DestroyArrow()
    {
        yield return new WaitForSeconds(destroyTime);
        Destroy(this.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Character player = collision.gameObject.GetComponent<Character>();
            Vector2 direction = (player.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right; // Find direction of knockback
            player.RecieveDamage(1, direction);
        }
    }
}
