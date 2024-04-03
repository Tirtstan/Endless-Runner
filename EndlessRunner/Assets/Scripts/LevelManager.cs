using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField]
    private GameObject[] levelPrefabs;

    [SerializeField]
    private Transform level;

    [SerializeField]
    private float difficultyIncreaseRate = 0.1f;
    public float CurrentLevelSpeed { get; set; } = 10f;

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
    }

    private void Update()
    {
        CurrentLevelSpeed += Time.deltaTime * difficultyIncreaseRate;
    }

    public void SpawnObstacle(Vector3 pos)
    {
        int index = Random.Range(0, levelPrefabs.Length);
        GameObject obj = Instantiate(levelPrefabs[index], pos, Quaternion.identity);
        obj.transform.SetParent(level);
    }

    public void DestroyAllObstacles()
    {
        for (int i = 0; i < level.childCount; i++)
        {
            Destroy(level.GetChild(i).gameObject);
        }
    }
}
