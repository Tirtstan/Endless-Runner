using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public static PauseMenu Instance { get; private set; }

    [Header("Components")]
    [SerializeField]
    private GameObject menu;

    [Header("Buttons")]
    [SerializeField]
    private Button resumeButton;

    [SerializeField]
    private Button restartButton;

    [SerializeField]
    private Button menuButton;

    [SerializeField]
    private Button exitButton;

    [SerializeField]
    private TextMeshProUGUI controlsText;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        resumeButton.onClick.AddListener(OnResumeClick);
        restartButton.onClick.AddListener(OnRestartClick);
        menuButton.onClick.AddListener(OnMenuClick);
        exitButton.onClick.AddListener(OnExitClick);
    }

    public void TogglePauseMenu()
    {
        if (menu.activeSelf)
        {
            controlsText.gameObject.SetActive(true);
            menu.SetActive(false);
            Time.timeScale = 1;
            GameManager.Instance.StartTimer();
        }
        else
        {
            controlsText.gameObject.SetActive(false);
            menu.SetActive(true);
            AudioManager.Instance.ToggleHighPassFilter(true);
            Time.timeScale = 0;
        }
    }

    private void OnResumeClick()
    {
        TogglePauseMenu();
    }

    private void OnRestartClick()
    {
        TogglePauseMenu();
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
}
