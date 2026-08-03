using System.Collections;
using UnityEngine;

public class Door : MonoBehaviour
{
    // Variables
    [SerializeField] private Player player;
    [SerializeField] private GameManager gameManager;
    [SerializeField] public bool open = false;
    [SerializeField] private Sprite openSprite;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private GameObject destroyText;
    [SerializeField] private GameObject enterText;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if (gameManager == null)
            gameManager = FindAnyObjectByType<GameManager>();

        StartCoroutine(Delay());

        if (player != null)
        {
            if (player.hasDoubleJump && player.hasSprint && player.hasDash && player.hasWallSlide)
            {
                open = true;
                spriteRenderer.sprite = openSprite;
                enterText.SetActive(true);
                destroyText.SetActive(false);
            }
            else
            {
                open = false;
                enterText.SetActive(false);
                destroyText.SetActive(true);
            }
        }
    }

    public void LoadScene()
    {
        gameManager.LoadScene("Tomb1", Vector2.zero);
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
