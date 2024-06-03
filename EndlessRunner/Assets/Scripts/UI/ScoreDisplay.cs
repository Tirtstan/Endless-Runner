using TMPro;
using UnityEngine;

public class ScoreDisplay : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        DatabaseManager.OnScoreChange += OnScoreChange;
    }

    private void OnScoreChange(int score)
    {
        scoreText.SetText($"Score: {score}");
    }

    private void OnDestroy()
    {
        DatabaseManager.OnScoreChange -= OnScoreChange;
    }
}
