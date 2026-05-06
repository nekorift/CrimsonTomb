using UnityEngine;

public class EnemyWalker : Character
{
    // Variables
    [SerializeField] private GameObject player;

    protected override void Start()
    {
        base.Start();

        player = FindPlayer();
        TurnToPlayer(player);
    }

    void FixedUpdate()
    {
        if (facingRight)
        {
            if (IsOnGround())
                body.AddForce(new Vector2(moveSpeed, 0), ForceMode2D.Force);

            if (body.linearVelocity.x > maxSpeed)
                body.linearVelocity = new Vector2(maxSpeed, body.linearVelocity.y);
        }
        else if (!facingRight)
        {
            if (IsOnGround())
                body.AddForce(new Vector2(-moveSpeed, 0), ForceMode2D.Force);

            if (body.linearVelocity.x < -maxSpeed)
                body.linearVelocity = new Vector2(-maxSpeed, body.linearVelocity.y);
        }
        else
            Debug.LogError("Facing direction not set correctly for " + gameObject.name);

        //if (body.linearVelocity.x < 0.5 && body.linearVelocity.x > -0.5f)
        if (IsTouchingWall() && body.linearVelocity.x == 0)
            facingRight = !facingRight;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Character player = collision.gameObject.GetComponent<Character>();
            Vector2 direction = (player.transform.position.x < transform.position.x) ? Vector2.left : Vector2.right; // Find direction of knockback
            player.RecieveDamage(baseDamage, direction);
        }
    }
}
