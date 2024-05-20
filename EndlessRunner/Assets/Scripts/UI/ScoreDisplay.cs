using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        GameManager.OnScoreChange += OnScoreChange;
    }

    private void OnScoreChange(int score)
    {
        scoreText.SetText($"Score: {score}");
    }

    private void OnDestroy()
    {
        GameManager.OnScoreChange -= OnScoreChange;
    }
}
