using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Variables
    [SerializeField] private Player player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private string scene;
    [SerializeField] public bool open = false;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (gameManager == null)
            gameManager = FindAnyObjectByType<GameManager>();

        if (player != null)
        {
            if (player.hasDoubleJump && player.hasSprint && player.hasDash && player.hasWallSlide)
            {
                open = true;
                spriteRenderer.sprite = openSprite;
            }
            else
            {
                open = false;
            }
        }
    }

    public void LoadScene()
    {
        gameManager.LoadScene(scene, Vector2.zero);
    }

    private IEnumerator Delay()
    {
        // Door wasn't interactable when player spawns while colliding with it
        Collider2D col = GetComponent<Collider2D>();
        col.enabled = false;
        yield return new WaitForSeconds(0.1f);
        col.enabled = true;
    }
}
