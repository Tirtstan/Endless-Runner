using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField]
    private GameObject[] levelPrefabs;

    [SerializeField]
    private GameObject levelTransitionPrefab;

    [SerializeField]
    private Transform levelParent;
    private List<GameObject> spawnedLevels = new(4);
    private bool isBossDefeated;

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

        if (levelParent == null)
        {
            Debug.LogWarning("Level Parent is not assigned in the inspector!", gameObject);
            return;
        }

        for (int i = 0; i < levelParent.childCount; i++)
            spawnedLevels.Add(levelParent.GetChild(i).gameObject);
    }

    private void Start()
    {
        EventManager.OnBossDefeated += OnBossDefeated;
    }

    private void Update()
    {
        if (isBossDefeated)
        {
            RenderSettings.fogStartDistance = 15f;
            RenderSettings.fogEndDistance = Mathf.Lerp(
                RenderSettings.fogEndDistance,
                RenderSettings.fogStartDistance * 2f,
                Time.deltaTime * 0.4f
            );
        }
    }

    private void OnBossDefeated(int bossId)
    {
        isBossDefeated = true;
    }

    public void SpawnObstacle()
    {
        GameObject levelPrefab = levelPrefabs[Random.Range(0, levelPrefabs.Length)];
        if (isBossDefeated)
            levelPrefab = levelTransitionPrefab;

        Vector3 lastLevel = spawnedLevels[^1].transform.position;

        GameObject obj = Instantiate(levelPrefab, new Vector3(lastLevel.x, lastLevel.y, lastLevel.z + 52f), Quaternion.identity);
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
            Destroy(levelParent.GetChild(i).gameObject);
    }

    private void OnDestroy()
    {
        EventManager.OnBossDefeated -= OnBossDefeated;
    }
}
