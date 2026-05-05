using System.Collections;
using UnityEngine;

public class EnemyWalker : Character
{
    // Variables
    [SerializeField] private GameObject player;

    protected override void Start()
    {
        base.Start();

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
            Debug.LogError("Player not found in the scene. Please ensure there is a GameObject with the tag 'Player'.");
        else if (player.transform.position.x > transform.position.x)
            facingRight = true;
        else
            facingRight = false;
    }

    void FixedUpdate()
    {
        CheckIsOnGround();

        if (facingRight)
        {
            if (isOnGround)
                body.AddForce(new Vector2(moveSpeed, 0), ForceMode2D.Force);

            if (body.linearVelocity.x > maxSpeed)
                body.linearVelocity = new Vector2(maxSpeed, body.linearVelocity.y);
        }
        else if (!facingRight)
        {
            if (isOnGround)
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
