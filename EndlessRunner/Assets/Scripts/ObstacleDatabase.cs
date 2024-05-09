using UnityEngine;

public class ObstacleDatabase : MonoBehaviour
{
    public static ObstacleDatabase Instance { get; private set; }

    [SerializeField]
    private GameObject[] lanePrefabs;
    private int previousIndex;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject GetRandomLanePrefab()
    {
        int index;
        do
        {
            index = Random.Range(0, lanePrefabs.Length);
        } while (previousIndex == index); // Prevents the same lane from spawning twice in a row

        previousIndex = index;
        return lanePrefabs[index];
    }
}
