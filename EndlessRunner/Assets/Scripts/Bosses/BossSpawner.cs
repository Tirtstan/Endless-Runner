using UnityEngine;
using UnityEngine.SceneManagement;

public class BossSpawner : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private GameObject[] bossPrefabs;

    [Header("Spawn Configs")]
    [SerializeField]
    [Range(10, 100)]
    private int spawnScoreThreshold = 30;

    [SerializeField]
    [Range(5, 30)]
    private int scoreMultipleCheck = 10;

    [SerializeField]
    [Range(1, 20)]
    private int spawnChanceIncrease = 5;
    private int spawnChance = 5;
    private GameObject boss;

    private void Start()
    {
        DatabaseManager.OnScoreChange += OnScoreChange;
    }

    private void OnScoreChange(int score)
    {
        if (boss != null)
            return;

        if (score < spawnScoreThreshold)
            return;

        if (score % scoreMultipleCheck == 0)
        {
            int random = Random.Range(0, 100);
            if (random <= spawnChance)
            {
                spawnChance = 0;
                int sceneIndex = SceneManager.GetActiveScene().buildIndex;
                switch (sceneIndex)
                {
                    case 1:
                        SpawnBoss1();
                        break;
                    case 2:
                        SpawnBoss2();
                        break;
                }
            }
            else
            {
                spawnChance += spawnChanceIncrease;
            }
        }
    }

    private void SpawnBoss1()
    {
        boss = Instantiate(bossPrefabs[0], new Vector3(1.5f, 5f, -10f), Quaternion.identity);
    }

    private void SpawnBoss2()
    {
        boss = Instantiate(bossPrefabs[1]);
    }

    private void OnDestroy()
    {
        DatabaseManager.OnScoreChange -= OnScoreChange;
    }
}
