using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private TextMeshProUGUI scoreText;
    private int score;

    private void Start()
    {
        ObstaclePass.OnObstaclePass += OnObstaclePass;
    }

    private void OnObstaclePass()
    {
        score++;
        scoreText.text = $"Score: {score}";
    }

    private void OnDestroy()
    {
        ObstaclePass.OnObstaclePass -= OnObstaclePass;
    }
}
