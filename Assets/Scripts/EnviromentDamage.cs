using System.Collections;
using UnityEngine;

public class EnviromentDamage : MonoBehaviour
{
    // Variables
    //[SerializeField] private bool isCausingDamage = false;
    //[SerializeField] private bool canDamage = true;
    [SerializeField] private bool playerTouching = false;
    [SerializeField] private float damageDelay = 1f;
    [SerializeField] private Character player;
    private Coroutine damageRoutine;

    private IEnumerator DealDamage()
    {
        //while (isCausingDamage && player != null)
        //{
        //    if (canDamage)
        //    {
        //        canDamage = false;
        //        player.currentHealth--;

        //        yield return new WaitForSeconds(damageDelay);
        //        canDamage = true;
        //    }
        //    yield return null;
        //}
        //damageRoutine = null;

        while (playerTouching && player != null)
        {
            if (!player.activeIframes)
            {
                //player.currentHealth--;
                player.RecieveDamage(1f, Vector2.zero);
                yield return new WaitForSeconds(damageDelay);
            }
            else
            {
                yield return null;
            }
        }

        damageRoutine = null;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player = collision.gameObject.GetComponent<Character>();
            //isCausingDamage = true;
            playerTouching = true;

            if (damageRoutine == null)
                damageRoutine = StartCoroutine(DealDamage());
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            playerTouching = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //isCausingDamage = false;
            playerTouching = false;
            player = null;
            //canDamage = true;

            //if (damageRoutine != null)
            //{
            //    StopCoroutine(damageRoutine);
            //    damageRoutine = null;
            //}
        }
    }
}
