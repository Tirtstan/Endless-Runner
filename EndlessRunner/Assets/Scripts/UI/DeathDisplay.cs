using TMPro;
using Unity.Services.Authentication;
using UnityEngine;
using UnityEngine.SceneManagement;
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
    }

    private void OnRestartClick()
    {
        GameManager.Instance.RestartGame();
        Time.timeScale = 1;
    }

    private void OnMenuClick()
    {
        SceneManager.LoadSceneAsync(0);
        Time.timeScale = 1;
    }

    private void OnExitClick()
    {
        Application.Quit();
    }

    private void FillInfo()
    {
        playerMetricsText.text =
            $"<size=+2><u>Current Session Stats:</u></size>\n\nPlayer: {AuthenticationService.Instance.PlayerName}\n"
            + $"{PlayerMetricsManager.Instance.GetSessionPlayerMetrics()}";
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerHealthChanged -= OnPlayerHit;
    }
}
