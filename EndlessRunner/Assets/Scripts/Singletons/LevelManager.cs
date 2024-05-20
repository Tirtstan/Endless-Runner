using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField]
    private GameObject[] levelPrefabs;

    [SerializeField]
    private Transform levelParent;

    [Header("Config")]
    [SerializeField]
    private float difficultyIncreaseRate = 0.05f;

    [SerializeField]
    private float levelSpeedCap = 22.5f;

    [field: SerializeField]
    public float CurrentLevelSpeed { get; private set; } = 10f;
    private List<GameObject> spawnedLevels = new(4);

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        for (int i = 0; i < levelParent.childCount; i++)
            spawnedLevels.Add(levelParent.GetChild(i).gameObject);
    }

    private void Update()
    {
        if (CurrentLevelSpeed >= levelSpeedCap)
            return;

        CurrentLevelSpeed += Time.deltaTime * difficultyIncreaseRate;
    }

    public void SpawnObstacle()
    {
        int index = Random.Range(0, levelPrefabs.Length);
        Vector3 lastLevel = spawnedLevels[^1].transform.position;

        GameObject obj = Instantiate(
            levelPrefabs[index],
            new Vector3(lastLevel.x, lastLevel.y, lastLevel.z + 52f),
            Quaternion.identity
        );
        obj.transform.SetParent(levelParent);

        spawnedLevels.Add(obj);
    }

    public void DestroyObstacle(GameObject obj)
    {
        spawnedLevels.Remove(obj);
        Destroy(obj, 1);
    }

    public void DestroyAllObstacles()
    {
        for (int i = 0; i < levelParent.childCount; i++)
        {
            Destroy(levelParent.GetChild(i).gameObject);
        }
    }
}
