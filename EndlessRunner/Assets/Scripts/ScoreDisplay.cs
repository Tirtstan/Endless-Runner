using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        ScoreTrigger.OnObstaclePass += OnObstaclePass;
    }

    private void OnObstaclePass()
    {
        scoreText.SetText($"Score: {GameManager.Instance.GetScore()}");
    }

    private void OnDestroy()
    {
        ScoreTrigger.OnObstaclePass -= OnObstaclePass;
    }
}
