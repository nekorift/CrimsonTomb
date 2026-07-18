using System.Collections;
using UnityEngine;

public class BossFireGiant : Character
{
    // Variables
    [SerializeField] private GameObject player;
    [SerializeField] public bool isAttacking = false;
    private Coroutine attackRoutine;
    [SerializeField] private BoxCollider2D dCol;

    protected override void Start()
    {
        base.Start();

        player = FindPlayer();
        TurnToPlayer(player);
        dCol = GetComponentInChildren<BoxCollider2D>();
    }

    private void FixedUpdate()
    {
        if (attackRoutine == null)
            attackRoutine = StartCoroutine(Attack());

        if (!isAttacking)
        {
            TurnToPlayer(player);

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

            baseDamage = 1f;
        }
        else
        {
            baseDamage = 2f;
        }

        if (facingRight)
            spriteRenderer.flipX = false;
        else
            spriteRenderer.flipX = true;

        if (!isAttacking)
        {
            dCol.offset = Vector2.zero;
            dCol.size = new Vector2(2f, 4f);
            animator.Play("Walking");
        }
        else if (isAttacking)
        {
            body.linearVelocity = new Vector2(0, body.linearVelocity.y);

            if (facingRight)
                dCol.offset = new Vector2(.6f, 0);
            else
                dCol.offset = new Vector2(-.6f, 0);

            dCol.size = new Vector2(2.3f, 4f);
            animator.Play("Attacking");
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(2f);

        int r = Random.Range(0, 3);
        if (r == 0)
        {
            isAttacking = true;
        }
        else
        {
            isAttacking = false;
        }

        attackRoutine = null;
    }
}
