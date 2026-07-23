using UnityEngine;

public class BloodArrowSpawner : MonoBehaviour
{
    // Variables
    [SerializeField] private GameObject bloodArrowPrefab;
    [SerializeField] private GameObject[] spawners;
    [SerializeField] private int exclusionRange = 5;

    private void Start()
    {
        SpawnBloodArrow(); // Testing purpose
    }

    public void SpawnBloodArrow()
    {
        //foreach (GameObject s in spawners)
        //{
        //    GameObject bloodArrow = Instantiate(bloodArrowPrefab, s.transform.position, Quaternion.identity);
        //    bloodArrow.transform.up = s.transform.up; // Set the arrow's up direction to match the spawner's up direction
        //}

        int excluded = Random.Range(0, exclusionRange);

        for (int i = 0; i < spawners.Length; i++)
        {
            if (i < excluded -1 || i > excluded + 1)
            {
                GameObject bloodArrow = Instantiate(bloodArrowPrefab, spawners[i].transform.position, Quaternion.identity);
                bloodArrow.transform.up = spawners[i].transform.up; // Set the arrow's up direction to match the spawner's up direction
            }
        }
    }
}
