using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        PlayerMetricsManager.OnScoreChange += OnScoreChange;
    }

    private void OnScoreChange(int score)
    {
        scoreText.SetText($"Score: {score}");
    }

    private void OnDestroy()
    {
        PlayerMetricsManager.OnScoreChange -= OnScoreChange;
    }
}
