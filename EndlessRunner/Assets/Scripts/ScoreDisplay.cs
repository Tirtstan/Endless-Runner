using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        ObstaclePass.OnObstaclePass += OnObstaclePass;
    }

    private void OnObstaclePass()
    {
        GameManager.Instance.IncreaseScore();
        scoreText.SetText($"Score: {GameManager.Instance.GetScore()}");
    }

    private void OnDestroy()
    {
        ObstaclePass.OnObstaclePass -= OnObstaclePass;
    }
}
