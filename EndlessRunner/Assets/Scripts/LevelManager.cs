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

    [field: SerializeField]
    public float CurrentLevelSpeed { get; private set; } = 10f;

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
        obj.transform.SetParent(levelParent);
    }

    public void DestroyAllObstacles()
    {
        for (int i = 0; i < levelParent.childCount; i++)
        {
            Destroy(levelParent.GetChild(i).gameObject);
        }
    }
}
