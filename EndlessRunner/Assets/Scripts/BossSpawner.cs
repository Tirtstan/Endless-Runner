using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField]
    private GameObject[] bossPrefabs;

    [Header("Spawn Configs")]
    [SerializeField]
    private int scoreMulitpleCheck = 30;

    [SerializeField]
    private int spawnChanceIncrease = 10;
    private int spawnChance = 5;
    private GameObject boss;

    private void Start()
    {
        ScoreTrigger.OnObstaclePass += OnObstaclePass;
    }

    private void OnObstaclePass()
    {
        if (boss != null)
            return;

        int currentScore = GameManager.Instance.GetScore();
        if (currentScore % scoreMulitpleCheck == 0)
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
        ScoreTrigger.OnObstaclePass -= OnObstaclePass;
    }
}
