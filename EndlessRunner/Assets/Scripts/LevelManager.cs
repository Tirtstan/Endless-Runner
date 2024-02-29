using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField]
    private GameObject[] levelPrefabs;
    [SerializeField]
    private Transform level;
    public float currentLevelSpeed = 10f;

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

    public void SpawnObstacle()
    {
        int index = Random.Range(0, levelPrefabs.Length);
        GameObject obj = Instantiate(levelPrefabs[index], new Vector3(0, 0, 100), Quaternion.identity);
        obj.transform.SetParent(level);
    }
}
