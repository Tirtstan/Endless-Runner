using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Leaderboards;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField]
    private GameObject optionsPanel;

    [Header("Main Menu")]
    [Header("Buttons")]
    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button optionsButton;

    [SerializeField]
    private Button statsButton;

    [SerializeField]
    private Button exitButton;

    [Header("Options Menu")]
    [SerializeField]
    private Button optionsBackButton;

    [Header("Stats Menu")]
    [Header("Buttons")]
    [SerializeField]
    private Button statsBackButton;

    [Header("Text")]
    [SerializeField]
    private TextMeshProUGUI statsText;

    [Header("Configs")]
    [SerializeField]
    private Vector2 mainPanelRot = new(-6, -12);

    [SerializeField]
    private Vector2 optionsPanelRot = new(-40, -12);

    [SerializeField]
    private Vector2 statsPanelRot = new(-6, 25);

    [SerializeField]
    [Range(1, 10)]
    private float rotSpeed = 4;
    private Camera mainCamera;
    private Vector2 targetRot;

    [SerializeField]
    private void Awake()
    {
        targetRot = mainPanelRot;
        mainCamera = Camera.main;

        playButton.onClick.AddListener(OnPlayClick);
        optionsButton.onClick.AddListener(OnOptionsClick);
        statsButton.onClick.AddListener(OnStatsClick);
        exitButton.onClick.AddListener(OnExitClick);

        optionsBackButton.onClick.AddListener(OnBackClick);
        statsBackButton.onClick.AddListener(OnBackClick);
    }

    private void Update()
    {
        mainCamera.transform.rotation = Quaternion.Slerp(
            mainCamera.transform.rotation,
            Quaternion.Euler(targetRot.x, targetRot.y, mainCamera.transform.eulerAngles.z),
            Time.deltaTime * rotSpeed
        );
    }

    private void OnPlayClick() => SceneManager.LoadSceneAsync(1);

    private void OnOptionsClick()
    {
        targetRot = optionsPanelRot;
        optionsPanel.SetActive(true);
    }

    private async void OnStatsClick()
    {
        targetRot = statsPanelRot;
        int highScore = await GetPlayerHighScore();
        statsText.text =
            $"Player: {AuthenticationService.Instance.PlayerName}\n"
            + $"{DatabaseManager.Instance.GetTotalPlayerMetrics()}\n"
            + $"\nHigh Score: {highScore}";
    }

    private async Task<int> GetPlayerHighScore()
    {
        var info = await LeaderboardsService.Instance.GetPlayerScoreAsync(
            DatabaseManager.LeaderboardId
        );

        return (int)info.Score;
    }

    private void OnExitClick() => Application.Quit();

    private void OnBackClick()
    {
        optionsPanel.SetActive(false);
        targetRot = mainPanelRot;
    }
}
