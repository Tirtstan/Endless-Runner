using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
            menu.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            menu.SetActive(true);
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
    }

    private void OnMenuClick()
    {
        SceneManager.LoadSceneAsync(0);
    }

    private void OnExitClick()
    {
        Application.Quit();
    }
}
