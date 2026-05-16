using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Vector2 minPos;
    [SerializeField] private Vector2 maxPos;

    private void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        // Update later to implement camera bounds
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
    }
}
