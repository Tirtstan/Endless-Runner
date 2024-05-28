using UnityEngine;

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
    private int scoreMulitpleCheck = 10;

    [SerializeField]
    [Range(1, 20)]
    private int spawnChanceIncrease = 5;
    private int spawnChance = 5;
    private GameObject boss;

    private void Start()
    {
        EventManager.OnScoreChange += OnScoreChange;
    }

    private void OnScoreChange(int score)
    {
        if (boss != null)
            return;

        if (score < spawnScoreThreshold)
            return;

        if (score % scoreMulitpleCheck == 0)
        {
            int random = Random.Range(0, 100);
            if (random <= spawnChance)
            {
                spawnChance = 0;
                int randomIndex = Random.Range(0, bossPrefabs.Length);
                boss = Instantiate(
                    bossPrefabs[randomIndex],
                    new Vector3(1.5f, 5f, -10f),
                    Quaternion.identity
                );
            }
            else
            {
                spawnChance += spawnChanceIncrease;
            }
        }
    }

    private void OnDestroy()
    {
        EventManager.OnScoreChange -= OnScoreChange;
    }
}
