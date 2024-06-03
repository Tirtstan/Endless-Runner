using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.UI;

public class DeathDisplay : MonoBehaviour
{
    [Header("Components")]
    [Header("Panels")]
    [SerializeField]
    private GameObject deathScreen;

    [SerializeField]
    private GameObject scoreDisplay;

    [Header("Text")]
    [SerializeField]
    private TextMeshProUGUI playerMetricsText;

    [Header("Buttons")]
    [SerializeField]
    private Button restartButton;

    [SerializeField]
    private Button menuButton;

    [SerializeField]
    private Button exitButton;

    private void Awake()
    {
        restartButton.onClick.AddListener(OnRestartClick);
        menuButton.onClick.AddListener(OnMenuClick);
        exitButton.onClick.AddListener(OnExitClick);
    }

    private void Start()
    {
        PlayerHealth.OnPlayerHealthChanged += OnPlayerHit;
    }

    private void OnPlayerHit(int currentHealth)
    {
        if (currentHealth > 0)
            return;

        Time.timeScale = 0;
        scoreDisplay.SetActive(false);
        deathScreen.SetActive(true);

        FillInfo();
        AddToLeaderboard();
    }

    private void OnRestartClick()
    {
        GameManager.Instance.RestartGame();
        Time.timeScale = 1;
    }

    private void OnMenuClick()
    {
        SceneTransitionManager.Instance.LoadScene(0, SceneTransitionManager.TransitionType.Top);
        Time.timeScale = 1;
    }

    private void OnExitClick()
    {
        Application.Quit();
    }

    private void FillInfo()
    {
        playerMetricsText.text =
            $"<size=+2><u>Session Stats:</u></size>\nPlayer: {AuthenticationService.Instance.PlayerName}\n"
            + $"{DatabaseManager.Instance.GetSessionPlayerMetrics()}";
    }

    private async void AddToLeaderboard()
    {
        try
        {
            var playerEntry = await LeaderboardsService.Instance.AddPlayerScoreAsync(
                DatabaseManager.LeaderboardId,
                DatabaseManager.Instance.GetSessionPlayerMetrics().Score
            );
            Debug.Log($"Added score to leaderboard!");
        }
        catch (System.Exception e)
        {
            Debug.LogWarning($"Failed to add score to leaderboard!: {e.Message}");
        }
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerHealthChanged -= OnPlayerHit;
    }
}
