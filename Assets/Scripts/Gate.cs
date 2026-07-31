using UnityEngine;

public class Gate : MonoBehaviour
{
    // Variables
    [SerializeField] private Player player;
    [SerializeField] private string ability;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        if (player != null)
        {
            if (ability == "jump")
            {
                if (player.hasDoubleJump)
                {
                    this.gameObject.SetActive(true);
                }
                else
                {
                    this.gameObject.SetActive(false);
                }
            }

            if (ability == "sprint")
            {
                if (player.hasSprint)
                {
                    this.gameObject.SetActive(true);
                }
                else
                {
                    this.gameObject.SetActive(false);
                }
            }
            
            if (ability == "dash")
            {
                if (player.hasDash)
                {
                    this.gameObject.SetActive(true);
                }
                else
                {
                    this.gameObject.SetActive(false);
                }
            }
            
            if (ability == "wall")
            {
                if (player.hasWallSlide)
                {
                    this.gameObject.SetActive(true);
                }
                else
                {
                    this.gameObject.SetActive(false);
                }
            }
        }
    }
}
