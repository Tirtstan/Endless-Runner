using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    public enum LaneType
    {
        None = 0,
        Left = 1,
        Middle = 2,
        Right = 3,
        LeftMiddle = 4,
        MiddleRight = 5,
        LeftRight = 6,
        All = 7
    }

    private GameObject[] laneSpawns;

    private void Awake()
    {
        laneSpawns = new GameObject[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
            laneSpawns[i] = transform.GetChild(i).gameObject;
    }

    private void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        int chance = Random.Range(0, 100);
        for (int i = 0; i < laneSpawns.Length; i++)
        {
            if (chance <= 10) // leaves a space
            {
                chance = 99;
                continue;
            }

            int rotationChance = Random.Range(0, 100);
            Quaternion rotation = rotationChance < 50 ? Quaternion.identity : Quaternion.Euler(0, 180, 0);

            GameObject obj = Instantiate(
                ObstacleDatabase.Instance.GetRandomLanePrefab(),
                laneSpawns[i].transform.position,
                rotation
            );
            obj.transform.SetParent(laneSpawns[i].transform);
        }
    }
}
