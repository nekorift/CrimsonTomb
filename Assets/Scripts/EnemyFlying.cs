using UnityEngine;

public class EnemyFlying : Character
{
    // Variables
    [SerializeField] private GameObject player;
    [SerializeField] private float top;
    [SerializeField] private float bottom;
    [SerializeField] private bool isGoingDown = true;

    protected override void Start()
    {
        base.Start();
        player = FindPlayer();
        TurnToPlayer(player);

        top = transform.position.y + jumpForce;
        bottom = transform.position.y - jumpForce;
    }

    void FixedUpdate()
    {
        if (facingRight)
        {
            body.AddForce(new Vector2(moveSpeed, 0), ForceMode2D.Force);

            if (body.linearVelocity.x > maxSpeed)
                body.linearVelocity = new Vector2(maxSpeed, body.linearVelocity.y);
        }
        else if (!facingRight)
        {
            body.AddForce(new Vector2(-moveSpeed, 0), ForceMode2D.Force);

            if (body.linearVelocity.x < -maxSpeed)
                body.linearVelocity = new Vector2(-maxSpeed, body.linearVelocity.y);
        }
        else
            Debug.LogError("Facing direction not set correctly for " + gameObject.name);

        if (isGoingDown)
        {
            body.AddForce(new Vector2(0, -moveSpeed), ForceMode2D.Force);

            if (body.linearVelocity.y < -maxSpeed)
                body.linearVelocity = new Vector2(body.linearVelocity.x, -maxSpeed);
        }
        else
        {
            body.AddForce(new Vector2(0, moveSpeed), ForceMode2D.Force);

            if (body.linearVelocity.y > maxSpeed)
                body.linearVelocity = new Vector2(body.linearVelocity.x, maxSpeed);
        }

        if (transform.position.y <= bottom)
        {
            //body.linearVelocity = new Vector2(body.linearVelocity.x, 0); // Reset vertical velocity to ensure consistent movement between direction changes
            isGoingDown = false;
            body.AddForce(new Vector2(0, moveSpeed), ForceMode2D.Force);
        }
        else if (transform.position.y >= top)
        {
            //body.linearVelocity = new Vector2(body.linearVelocity.x, 0); // Reset vertical velocity to ensure consistent movement between direction changes
            isGoingDown = true;
            body.AddForce(new Vector2(0, -moveSpeed), ForceMode2D.Force);
        }
    }
}
