using System;
using System.Threading.Tasks;
using TMPro;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Leaderboards;
using UnityEngine;
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

    [SerializeField]
    private TextMeshProUGUI highScoresText;

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

    private void Start()
    {
        AuthenticationService.Instance.SignedIn += OnSignedIn;
        if (AuthenticationService.Instance.IsSignedIn)
            OnSignedIn();
    }

    private void Update()
    {
        mainCamera.transform.rotation = Quaternion.Slerp(
            mainCamera.transform.rotation,
            Quaternion.Euler(targetRot.x, targetRot.y, mainCamera.transform.eulerAngles.z),
            Time.deltaTime * rotSpeed
        );
    }

    private void OnPlayClick()
    {
        SceneTransitionManager.Instance.LoadScene(1, SceneTransitionManager.TransitionType.Top);
        EventManager.Instance.InvokeAttempt();
    }

    private void OnOptionsClick()
    {
        targetRot = optionsPanelRot;
        optionsPanel.SetActive(true);
    }

    private void OnStatsClick()
    {
        if (UnityServices.State == ServicesInitializationState.Uninitialized)
            DatabaseManager.Instance.Initialize();

        targetRot = statsPanelRot;
        OnSignedIn();
    }

    private async void OnSignedIn()
    {
        SetStats();

        await DatabaseManager.Instance.LoadPlayerMetrics();
        SetLeaderboard();
    }

    // According to Unity (s.a) ...
    private async void SetStats()
    {
        int highScore = await GetPlayerHighScore();
        statsText.text =
            $"Player: {AuthenticationService.Instance.PlayerName}\n"
            + $"{DatabaseManager.Instance.GetTotalPlayerMetrics()}\n"
            + $"\nHigh Score: {highScore}";
    }

    // According to Unity (s.a) ...
    private async void SetLeaderboard()
    {
        try
        {
            var scoresResponse = await LeaderboardsService.Instance.GetScoresAsync(
                DatabaseManager.LeaderboardId,
                new GetScoresOptions { Offset = 0, Limit = 10 }
            );

            string output = "";
            for (int i = 0; i < 10; i++)
            {
                string info = ".............";
                if (i < scoresResponse.Results.Count)
                {
                    string name = scoresResponse.Results[i].PlayerName;
                    string usingName = name.Length > 10 ? name.Substring(0, 10) + "..." : name;

                    int score = Mathf.Clamp((int)scoresResponse.Results[i].Score, 0, 100000);
                    string scoreDisplay = score.ToString();
                    if (score >= 100000)
                        scoreDisplay = "99999+";

                    info = $"{usingName} - <size=+0.5>{scoreDisplay}</size>";
                    if (scoresResponse.Results[i].PlayerId == AuthenticationService.Instance.PlayerId)
                    {
                        info = "<u>" + info + "</u>";
                    }
                }

                output += $"{i + 1}| {info}\n";
            }

            highScoresText.text = output;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to get scores!: {e.Message}");
        }
    }

    // According to Unity (s.a) ...
    private async Task<int> GetPlayerHighScore()
    {
        try
        {
            var info = await LeaderboardsService.Instance.GetPlayerScoreAsync(DatabaseManager.LeaderboardId);
            return (int)info.Score;
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to get player high score!: {e.Message}");
        }

        return 0;
    }

    private void OnExitClick() => Application.Quit();

    private void OnBackClick()
    {
        optionsPanel.SetActive(false);
        targetRot = mainPanelRot;
    }

    private void OnDestroy()
    {
        AuthenticationService.Instance.SignedIn -= OnSignedIn;
    }
}

#region References
/*

Unity. s.a. Get scores, n.d. [Online]. Available at: https://docs.unity.com/ugs/en-us/manual/leaderboards/manual/get-score [Accessed 01 June 2024]

Unity. s.a. Get the player’s score, n.d. [Online]. Available at: https://docs.unity.com/ugs/en-us/manual/leaderboards/manual/get-player-score [Accessed 01 June 2024]

*/
#endregion
