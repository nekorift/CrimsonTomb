using System.Collections;
using UnityEngine;

public class BossPaladin : Character
{
    // Variables
    [SerializeField] private GameObject player;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool isIdle = false;
    private Coroutine attackRoutine;
    [SerializeField] private BoxCollider2D dCol;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameObject hammer;
    [SerializeField] private GameObject gate;

    protected override void Start()
    {
        base.Start();

        player = FindPlayer();
        TurnToPlayer(player);
    }

    private void FixedUpdate()
    {
        if (attackRoutine == null && !isAttacking && !isIdle)
            attackRoutine = StartCoroutine(AttackRoutine());

        if (!isAttacking && !isIdle)
        {
            TurnToPlayer(player);
        }
        //else if (isIdle)
        //{
        // Tried to make the shield effective by only letting the player attack from behind, but the code didn't work for some reason.
        // Just turning the iframes bool on would work, but then they would have to be turned off manually when behind the paladin, which would override the iframe coroutine, making the boss trivial.
        //if (facingRight)
        //{
        //    if (player.transform.position.x > transform.position.x)
        //    {
        //        Iframe();
        //    }
        //}
        //else if (!facingRight)
        //{
        //    if (player.transform.position.x < transform.position.x)
        //    {
        //        Iframe();
        //    }
        //}
        //}
        //else if (isAttacking)
        //{

        //}

        if (!isIdle)
        {
            Iframe();
        }

        if (facingRight)
        {
            spriteRenderer.flipX = false;
            col.offset = new Vector2(.2f, col.offset.y);
            dCol.offset = new Vector2(.2f, col.offset.y);
        }
        else
        {
            spriteRenderer.flipX = true;
            col.offset = new Vector2(-.2f, col.offset.y);
            dCol.offset = new Vector2(-.2f, col.offset.y);
        }

        if (!isAttacking && !isIdle)
        {
            spriteRenderer.sprite = sprites[0];
        }
        else if (isAttacking)
        {
            spriteRenderer.sprite = sprites[1];
        }
        else if (isIdle)
        {
            spriteRenderer.sprite = sprites[2];
        }
    }

    private IEnumerator AttackRoutine()
    {
        int r = Random.Range(0, 3);

        if (r == 0)
        {
            isAttacking = true;
        }

        yield return new WaitForSeconds(2f);

        if (isAttacking)
        {
            Attack();
            isAttacking = false;
            isIdle = true;
        }

        attackRoutine = null;
    }

    private void Attack()
    {
        GameObject h;
        PaladinHammer ph;

        if (player.transform.position.y > transform.position.y && player.transform.position.x > transform.position.x - 2 && player.transform.position.x < transform.position.x + 2)
        {
            // Attack up
            // instantiate
            h = Instantiate(hammer, new Vector2(transform.position.x, transform.position.y + 2), Quaternion.identity);
            ph = h.GetComponent<PaladinHammer>();
            // call method in hammer to set direction
            ph.target = new Vector2(transform.position.x, transform.position.y + 7);
            ph.origin = transform.position;
        }
        else if (facingRight)
        {
            // instantiate
            h = Instantiate(hammer, new Vector2(transform.position.x + 2, transform.position.y), Quaternion.identity);
            ph = h.GetComponent<PaladinHammer>();
            // call method in hammer to set direction
            ph.target = new Vector2(transform.position.x + 7, transform.position.y);
            ph.origin = transform.position;
        }
        else if (!facingRight)
        {
            // instantiate
            h = Instantiate(hammer, new Vector2(transform.position.x - 2, transform.position.y), Quaternion.identity);
            ph = h.GetComponent<PaladinHammer>();
            // call method in hammer to set direction
            ph.target = new Vector2(transform.position.x - 7, transform.position.y);
            ph.origin = transform.position;
        }
    }

    public void Res()
    {
        isAttacking = false;
        isIdle = false;
    }

    public void Death()
    {
        Destroy(gate);
        Destroy(gameObject);
    }
}
