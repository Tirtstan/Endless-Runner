using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private GameObject[] pickupPrefabs;

    [SerializeField]
    private GameObject pickupSpawnPointsParent;
    private GameObject[] pickupSpawnPoints;
    private static int spawnPercentage = 20;

    private void Awake()
    {
        pickupSpawnPoints = new GameObject[pickupSpawnPointsParent.transform.childCount];
        for (int i = 0; i < pickupSpawnPointsParent.transform.childCount; i++)
        {
            pickupSpawnPoints[i] = pickupSpawnPointsParent.transform.GetChild(i).gameObject;
        }

        int spawnChance = Random.Range(0, 101);
        if (spawnChance <= spawnPercentage) // 10% chance to spawn a pickup
        {
            SpawnPickup();
        }
    }

    private void SpawnPickup()
    {
        int pickupIndex = Random.Range(0, pickupPrefabs.Length);
        int spawnIndex = Random.Range(0, pickupSpawnPoints.Length);
        GameObject obj = Instantiate(
            pickupPrefabs[pickupIndex],
            pickupSpawnPoints[spawnIndex].transform.position,
            Quaternion.identity
        );

        obj.transform.SetParent(transform);
    }
}
