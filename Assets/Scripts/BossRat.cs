using System.Collections;
using UnityEngine;

public class BossRat : Character
{
    // Variables
    [SerializeField] private GameObject player;
    [SerializeField] public bool isAttacking = false;
    private Coroutine attackRoutine;
    [SerializeField] private CapsuleCollider2D dCol;

    protected override void Start()
    {
        base.Start();

        player = FindPlayer();
        TurnToPlayer(player);
    }

    private void FixedUpdate()
    {
        if (attackRoutine == null)
            attackRoutine = StartCoroutine(Attack());

        if (isAttacking)
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
        }
        else if (!isAttacking)
        {
            TurnToPlayer(player);
        }

        if (facingRight)
        {
            spriteRenderer.flipX = false;
            col.offset = new Vector2(.9f, 0);
            dCol.offset = new Vector2(.9f, 0);
        }
        else
        {
            spriteRenderer.flipX = true;
            col.offset = new Vector2(-.9f, 0);
            dCol.offset = new Vector2(-.9f, 0);
        }

        if (!isAttacking)
        {
            animator.Play("Standing");
        }
        else if (isAttacking)
        {
            animator.Play("Running");
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(2f);

        int r = Random.Range(0, 3);
        if (r == 0)
        {
            isAttacking = false;
        }
        else
        {
            isAttacking = true;
        }

        if (IsTouchingWall() && IsOnGround())
        {
            isAttacking = false;
        }

        attackRoutine = null;
    }
}
