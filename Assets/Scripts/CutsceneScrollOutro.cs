using UnityEngine;
using UnityEngine.SceneManagement;

public class CutsceneScrollOutro : MonoBehaviour
{
    // Variables
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RectTransform text;
    [SerializeField] private RectTransform scroll;
    [SerializeField] private float top;
    [SerializeField] private float bottom;
    [SerializeField] private float scrollSpeed = 30f;

    void Start()
{
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Destroy(player);

        gameManager = FindAnyObjectByType<GameManager>();
        scroll = GetComponent<RectTransform>();

        RectTransform canvas = GetComponentInParent<Canvas>().GetComponent<RectTransform>();

        Vector3[] canvasCorners = new Vector3[4];
        canvas.GetWorldCorners(canvasCorners);

        top = canvasCorners[1].y; // top left corner
    }

    void Update()
    {
        scroll.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        Vector3[] textCorners = new Vector3[4];
        text.GetWorldCorners(textCorners);

        bottom = textCorners[0].y; // bottom left corner

        if (bottom >= top)
        {
            Destroy(gameManager);
            SceneManager.LoadScene("MainMenu");
        }
    }
}
