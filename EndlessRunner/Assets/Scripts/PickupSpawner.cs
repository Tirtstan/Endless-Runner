using UnityEngine;

public class PickupSpawner : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private GameObject pickupSpawnPointsParent;
    private GameObject[] pickupSpawnPoints;

    private void Awake()
    {
        pickupSpawnPoints = new GameObject[pickupSpawnPointsParent.transform.childCount];
        for (int i = 0; i < pickupSpawnPointsParent.transform.childCount; i++)
        {
            pickupSpawnPoints[i] = pickupSpawnPointsParent.transform.GetChild(i).gameObject;
        }
    }

    private void Start()
    {
        int spawnChance = Random.Range(0, 100);
        if (spawnChance <= PickupManager.Instance.SpawnPercentage) // certain chance to spawn a pickup
        {
            SpawnPickup();
        }
    }

    private void SpawnPickup()
    {
        if (pickupSpawnPoints.Length <= 0)
            return;

        int spawnIndex = Random.Range(0, pickupSpawnPoints.Length);
        Vector3 pos = pickupSpawnPoints[spawnIndex].transform.position;

        GameObject obj = Instantiate(
            PickupManager.Instance.GetRandomPickup(),
            new Vector3(pos.x, pos.y + 0.5f, pos.z),
            Quaternion.identity
        );
        obj.transform.SetParent(transform);
    }
}
