using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Components")]
    [Header("Menus")]
    [SerializeField]
    private GameObject mainPanel;

    [SerializeField]
    private GameObject statsPanel;

    [Header("Buttons")]
    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button statsButton;

    [SerializeField]
    private Button exitButton;

    [SerializeField]
    private Button backButton;

    [SerializeField]
    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayClick);
        statsButton.onClick.AddListener(OnStatsClick);
        exitButton.onClick.AddListener(OnExitClick);
        backButton.onClick.AddListener(OnBackClick);

        OnBackClick();
    }

    private void OnPlayClick()
    {
        SceneManager.LoadSceneAsync(1);
    }

    private void OnStatsClick()
    {
        mainPanel.SetActive(false);
        statsPanel.SetActive(true);
    }

    private void OnExitClick()
    {
        Application.Quit();
    }

    private void OnBackClick()
    {
        mainPanel.SetActive(true);
        statsPanel.SetActive(false);
    }
}
