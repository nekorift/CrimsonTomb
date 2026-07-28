using System.Collections;
using UnityEngine;

public class BossPresence1 : Character
{
    // Variables
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] perches;
    [SerializeField] private BloodArrowSpawner[] arrowSpawners; // 0 is a child making a ring around the boss
    [SerializeField] private int currentPerch = 0;
    [SerializeField] private bool isAttacking = false;
    private Coroutine attackRoutine;
    private Coroutine arrowsRoutine;
    [SerializeField] private GameObject phase2;

    protected override void Start()
    {
        base.Start();

        player = FindPlayer();
        isAttacking = false;
    }

    private void FixedUpdate()
    {
        if (attackRoutine == null)
            attackRoutine = StartCoroutine(Attack());

        if (!isAttacking)
        {
            if (arrowsRoutine == null)
                arrowsRoutine = StartCoroutine(Arrows());

            if (currentPerch == 0)
            {
                body.position = Vector2.MoveTowards(transform.position, perches[0].transform.position, moveSpeed * Time.fixedDeltaTime);
            }
            else if (currentPerch == 1)
            {
                body.position = Vector2.MoveTowards(transform.position, perches[1].transform.position, moveSpeed * Time.fixedDeltaTime);
            }

            if (body.transform.position == perches[currentPerch].transform.position)
            {
                if (currentPerch == 0)
                {
                    currentPerch = 1;
                }
                else if (currentPerch == 1)
                {
                    currentPerch = 0;
                }
            }
        }
    }

    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(2f);

        int r = Random.Range(0, 3);
        if (r == 0)
        {
            isAttacking = true;
            yield return new WaitForSeconds(5);

            arrowSpawners[0].SpawnBloodArrow();

            yield return new WaitForSeconds(8);
            isAttacking = false;
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
        Instantiate(phase2, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
