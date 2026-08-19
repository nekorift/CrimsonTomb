using UnityEngine;

public class EnemyDeathSfx : MonoBehaviour
{
    [SerializeField] private AudioSource deathSfx;

    void Start()
    {
        deathSfx = GetComponent<AudioSource>();

        deathSfx.Play();
        Destroy(gameObject, deathSfx.clip.length);
    }
}
