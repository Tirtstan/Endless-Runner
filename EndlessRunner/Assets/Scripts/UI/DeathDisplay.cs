using TMPro;
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
    private TextMeshProUGUI playerNameText;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TextMeshProUGUI levelsBeatenText;

    [SerializeField]
    private TextMeshProUGUI jumpsText;

    [SerializeField]
    private TextMeshProUGUI jetpackPickupText;

    [SerializeField]
    private TextMeshProUGUI lowGravityPickupText;

    [SerializeField]
    private TextMeshProUGUI healPickupText;

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
        playerNameText.text = $"Player: {PlayerMetricsManager.Instance.CurrentPlayerName}";
        scoreText.text = $"Score: {PlayerMetricsManager.Instance.Score}";
        levelsBeatenText.text = $"Levels Beaten: {PlayerMetricsManager.Instance.LevelsBeaten}";
        jumpsText.text = $"Jumps: {PlayerMetricsManager.Instance.Jumps}";
        jetpackPickupText.text = $"Jetpacks: {PlayerMetricsManager.Instance.JetpackPickupAmount}";
        lowGravityPickupText.text =
            $"Low Gravity: {PlayerMetricsManager.Instance.LowGravityPickupAmount}";
        healPickupText.text = $"Heals: {PlayerMetricsManager.Instance.HealPickupAmount}";
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerHealthChanged -= OnPlayerHit;
    }
}
