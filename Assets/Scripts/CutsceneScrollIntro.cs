using UnityEngine;

public class CutsceneScrollIntro : MonoBehaviour
{
    // Variables
    [SerializeField] private GameManager gameManager;
    [SerializeField] private RectTransform panel;
    [SerializeField] private RectTransform text;
    [SerializeField] private RectTransform scroll;
    [SerializeField] private float top;
    [SerializeField] private float bottom;
    [SerializeField] private float scrollSpeed = 30f;

    void Start()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        scroll = GetComponent<RectTransform>();

        Vector3[] panelCorners = new Vector3[4];
        panel.GetWorldCorners(panelCorners);

        top = panelCorners[1].y; // top left corner
    }

    void Update()
    {
        scroll.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        Vector3[] textCorners = new Vector3[4];
        text.GetWorldCorners(textCorners);

        bottom = textCorners[0].y; // bottom left corner

        if (bottom >= top)
        {
            gameManager.StartNewGame();
        }
    }
}
