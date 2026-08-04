using System.Collections;
using UnityEngine;

public class BossBat : Character
{
    // Variables
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] perches;
    [SerializeField] private int currentPerch = -1;
    [SerializeField] private bool isPerched = false;
    [SerializeField] public bool isAttacking = false;
    private Coroutine perchRoutine;
    [SerializeField] private GameObject gate;

    protected override void Start()
    {
        base.Start();

        player = FindPlayer();
        currentPerch = Random.Range(0, perches.Length);
        isPerched = false;
        isAttacking = false;
    }

    void FixedUpdate()
    {
        if (!isPerched && !isAttacking)
        {
            if (currentPerch == -1)
                currentPerch = Random.Range(0, perches.Length);

            Vector2 targetPosition = perches[currentPerch].transform.position;
            body.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.fixedDeltaTime);

            if (Vector2.Distance(body.position, targetPosition) < 0.05f)
            {
                body.position = targetPosition;
                isPerched = true;
            }
        }
        else if (isPerched)
        {
            if (perchRoutine == null)
                perchRoutine = StartCoroutine(Perch());
        }
        else if (isAttacking)
        {
            body.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.fixedDeltaTime);
        }

        if (!isPerched)
            animator.Play("Flying");
        else if (isPerched)
            animator.Play("Perched");

        if (activeIframes)
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f, 1f);
        }
        else
        {
            GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
        }
    }

    private IEnumerator Perch()
    {
        yield return new WaitForSeconds(2f);

        int r = Random.Range(0, 5);
        if (r == 0)
        {
            isPerched = false;
            currentPerch = -1;
        }
        else if (r == 1 || r == 2)
        {
            isPerched = false;
            isAttacking = true;
            currentPerch = -1;
        }

        perchRoutine = null;
    }

    public void Death()
    {
        Destroy(gate);
        Destroy(gameObject);
    }
}
