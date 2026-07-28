using System.Collections;
using UnityEngine;

public class BossPresence2 : Character
{
    // Variables
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] perches;
    [SerializeField] private BloodArrowSpawner[] arrowSpawners; // 0 is a child making a ring around the boss
    [SerializeField] private int currentPerch = 0;
    [SerializeField] private bool isAttacking = false;
    [SerializeField] private bool isEmerging = false;
    [SerializeField] private bool isSubmerging = false;
    private Coroutine attackRoutine;
    private Coroutine arrowsRoutine;
    [SerializeField] private GameObject gate;

    protected override void Start()
    {
        base.Start();

        player = FindPlayer();
        isAttacking = false;
        isEmerging = false;
        isSubmerging = false;
    }

    private void FixedUpdate()
    {
        if (attackRoutine == null && !isSubmerging && !isEmerging)
            attackRoutine = StartCoroutine(Attack());

        if (!isAttacking && !isEmerging && !isSubmerging)
        {
            if (arrowsRoutine == null)
                arrowsRoutine = StartCoroutine(Arrows());

            if (currentPerch == 0)
            {
                body.AddForce(new Vector2(-moveSpeed, 0), ForceMode2D.Force);
            }
            else if (currentPerch == 1)
            {
                body.AddForce(new Vector2(moveSpeed, 0), ForceMode2D.Force);
            }

            
            if (currentPerch == 0)
            {
                if (body.position.x <= perches[0].transform.position.x)
                    currentPerch = 1;
            }
            else if (currentPerch == 1)
            {
                if (body.position.x >= perches[1].transform.position.x)
                    currentPerch = 0;
            }
        }

        if (!isAttacking && !isEmerging && !isSubmerging)
        {
            animator.Play("Crawl");
        }
        else if (isEmerging)
        {
            animator.Play("Emerge");
        }
        else if (isSubmerging)
        {
            animator.Play("Submerge");
        }
        else if (isAttacking)
        {
            animator.Play("AttackPhase");
        }
    }

    public void ChangeAnimation()
    {
        if (isEmerging)
        {
            col.enabled = true;
            isEmerging = false;
            isAttacking = true;
        }
        else if (isSubmerging)
        {
            isSubmerging = false;
            animator.Play("Crawl");
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(2f);

        int r = Random.Range(0, 3);
        if (r == 0)
        {
            if (!isAttacking && !isEmerging && !isSubmerging)
            {
                isEmerging = true;
            }
            else if (isAttacking)
            {
                col.enabled = false;
                isSubmerging = true;
                isAttacking = false;
            }
        }

        attackRoutine = null;
    }

    private IEnumerator Arrows()
    {
        yield return new WaitForSeconds(10f);

        int r = Random.Range(1, arrowSpawners.Length);
        arrowSpawners[r].SpawnBloodArrow();

        arrowsRoutine = null;
    }

    public void Death()
    {
        Destroy(gate);
        Destroy(gameObject);
    }
}
