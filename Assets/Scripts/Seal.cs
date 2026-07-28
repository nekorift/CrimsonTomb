using System.Collections;
using UnityEngine;

public class Seal : MonoBehaviour
{
    // Variables
    [SerializeField] private Sprite broken;
    [SerializeField] private GameManager gameManager;

    private void Start()
    {
        if (gameManager == null)
            gameManager = FindAnyObjectByType<GameManager>();
    }

    public void BreakSeal()
    {
        GetComponent<SpriteRenderer>().sprite = broken;
        StartCoroutine(Delay());
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(3f);
        gameManager.LoadScene("Hub", Vector2.zero);
    }
}
