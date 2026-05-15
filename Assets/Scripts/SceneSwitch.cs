using UnityEngine;

public class SceneSwitch : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private string sceneName;
    [SerializeField] private Vector2 spawnLocation;
    [SerializeField] private bool facingRight;

    private void Start()
    {
        if (gameManager == null)
            gameManager = FindAnyObjectByType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gameManager.LoadScene(sceneName, spawnLocation, facingRight);
        }
    }
}
