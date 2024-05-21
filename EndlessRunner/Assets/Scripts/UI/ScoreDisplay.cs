using UnityEngine;
using TMPro;

public class ScoreDisplay : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        EventManager.OnScoreChange += OnScoreChange;
    }

    private void OnScoreChange(int score)
    {
        scoreText.SetText($"Score: {score}");
    }

    private void OnDestroy()
    {
        EventManager.OnScoreChange -= OnScoreChange;
    }
}
