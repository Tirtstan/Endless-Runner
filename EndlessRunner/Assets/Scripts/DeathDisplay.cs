using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathDisplay : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private GameObject deathScreen;

    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private Button restartButton;

    [SerializeField]
    private Button menuButton;

    [SerializeField]
    private Button exitButton;

    [SerializeField]
    private GameObject scoreDisplay;

    private void Start()
    {
        PlayerController.OnPlayerDeath += OnPlayerDeath;
        restartButton.onClick.AddListener(OnRestartClick);
        menuButton.onClick.AddListener(OnMenuClick);
        exitButton.onClick.AddListener(OnExitClick);
    }

    private void OnPlayerDeath()
    {
        Time.timeScale = 0;
        scoreDisplay.SetActive(false);
        deathScreen.SetActive(true);
        scoreText.text = $"Score: {GameManager.Instance.GetScore()}";
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

    private void OnDestroy()
    {
        PlayerController.OnPlayerDeath -= OnPlayerDeath;
    }
}
