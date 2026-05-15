using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Player player;

    private void Start()
    {
        DontDestroyOnLoad(this);
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    public void LoadScene(string scene, Vector2 location, bool facingRight)
    {
        player.spawnLocation = location;
        SceneManager.LoadScene(scene);
        player.transform.position = location;
        player.facingRight = facingRight;
    }
}
