using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using Unity.Services.Core;
using Unity.Services.Core.Environments;
using UnityEngine;

public class DatabaseManager : MonoBehaviour
{
    public static DatabaseManager Instance { get; private set; }
    public static event Action<int> OnScoreChange;
    public string CurrentPlayerName { get; private set; } = "Player";
    private PlayerMetrics totalPlayerMetrics = new();
    private PlayerMetrics sessionPlayerMetrics = new();
    public const string LeaderboardId = "High_Scores";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Initialize();
    }

    public async void Initialize()
    {
        var options = new InitializationOptions();
        string environment = "production";

#if UNITY_EDITOR
        environment = "experimental";
        Debug.Log("Using experimental environment");
#endif

        options.SetEnvironmentName(environment);

        try
        {
            await UnityServices.InitializeAsync(options);
            Debug.Log("Unity Services initialized successfully!");

            Login();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to initialize Unity Services!: {e.Message}");
        }
    }

    private async void Login()
    {
        try
        {
            if (AuthenticationService.Instance.IsSignedIn)
                return;

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log(
                $"User signed in anonymously as {AuthenticationService.Instance.PlayerName} with ID: {AuthenticationService.Instance.PlayerId}"
            );
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to sign in anonymously!: {e.Message}");
        }
    }

    /// <summary>
    /// Returns the metrics data for a player session/run.
    /// </summary>
    public PlayerMetrics GetSessionPlayerMetrics() => sessionPlayerMetrics;

    /// <summary>
    /// Returns the metrics data for a player's lifetime.
    /// </summary>
    public PlayerMetrics GetTotalPlayerMetrics() => totalPlayerMetrics;

    private void Start()
    {
        EventManager.OnBossDefeated += OnBossDefeated;
        EventManager.OnPickup1 += OnPickup1;
        EventManager.OnPickup2 += OnPickup2;
        EventManager.OnPickup3 += OnPickup3;
        EventManager.OnAttempt += OnAttempt;

        PlayerController.OnPlayerJump += OnPlayerJump;
        PlayerHealth.OnPlayerHealthChanged += OnPlayerHealthChanged;

        AuthenticationService.Instance.SignedIn += OnSignedIn;
    }

    private async void OnSignedIn()
    {
        await LoadPlayerMetrics();
    }

    private void OnPlayerHealthChanged(int health) // cloud save when player dies
    {
        if (health > 0)
            return;

        SavePlayerMetrics();
    }

    private async void SavePlayerMetrics()
    {
        await LoadPlayerMetrics();

        totalPlayerMetrics.Attempts += sessionPlayerMetrics.Attempts;
        totalPlayerMetrics.Score += sessionPlayerMetrics.Score;
        totalPlayerMetrics.LevelsBeaten += sessionPlayerMetrics.LevelsBeaten;
        totalPlayerMetrics.Jumps += sessionPlayerMetrics.Jumps;
        totalPlayerMetrics.JetpackPickupAmount += sessionPlayerMetrics.JetpackPickupAmount;
        totalPlayerMetrics.LowGravityPickupAmount += sessionPlayerMetrics.LowGravityPickupAmount;
        totalPlayerMetrics.HealPickupAmount += sessionPlayerMetrics.HealPickupAmount;

        // Unity (s.a) demonstrates...
        var playerData = new Dictionary<string, object>
        {
            { "Attempts", totalPlayerMetrics.Attempts },
            { "Score", totalPlayerMetrics.Score },
            { "LevelsBeaten", totalPlayerMetrics.LevelsBeaten },
            { "Jumps", totalPlayerMetrics.Jumps },
            { "JetpackPickupAmount", totalPlayerMetrics.JetpackPickupAmount },
            { "LowGravityPickupAmount", totalPlayerMetrics.LowGravityPickupAmount },
            { "HealPickupAmount", totalPlayerMetrics.HealPickupAmount }
        };

        try
        {
            await CloudSaveService.Instance.Data.Player.SaveAsync(playerData);
            Debug.Log("Player data saved!");
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to save player data!: {e.Message}");
        }
    }

    // Unity (s.a) demonstrates...
    private async Task LoadPlayerMetrics()
    {
        try
        {
            var playerData = await CloudSaveService.Instance.Data.Player.LoadAllAsync();
            if (playerData == null)
            {
                Debug.LogWarning("Player data is null!");
                return;
            }

            if (playerData.TryGetValue("Attempts", out var attempts))
                totalPlayerMetrics.Attempts = attempts.Value.GetAs<int>();

            if (playerData.TryGetValue("Score", out var score))
                totalPlayerMetrics.Score = score.Value.GetAs<int>();

            if (playerData.TryGetValue("LevelsBeaten", out var levelsBeaten))
                totalPlayerMetrics.LevelsBeaten = levelsBeaten.Value.GetAs<int>();

            if (playerData.TryGetValue("Jumps", out var jumps))
                totalPlayerMetrics.Jumps = jumps.Value.GetAs<int>();

            if (playerData.TryGetValue("JetpackPickupAmount", out var jetpackPickupAmount))
                totalPlayerMetrics.JetpackPickupAmount = jetpackPickupAmount.Value.GetAs<int>();

            if (playerData.TryGetValue("LowGravityPickupAmount", out var lowGravityPickupAmount))
                totalPlayerMetrics.LowGravityPickupAmount = lowGravityPickupAmount.Value.GetAs<int>();

            if (playerData.TryGetValue("HealPickupAmount", out var healPickupAmount))
                totalPlayerMetrics.HealPickupAmount = healPickupAmount.Value.GetAs<int>();

            Debug.Log("Player data loaded!");
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to load player data!: {e.Message}");
        }
    }

    private void OnAttempt()
    {
        sessionPlayerMetrics.Attempts = 1;
    }

    private void OnPickup1()
    {
        sessionPlayerMetrics.JetpackPickupAmount++;
    }

    private void OnPickup2()
    {
        sessionPlayerMetrics.LowGravityPickupAmount++;
    }

    private void OnPickup3()
    {
        sessionPlayerMetrics.HealPickupAmount++;
    }

    private void OnPlayerJump()
    {
        sessionPlayerMetrics.Jumps++;
    }

    private void OnBossDefeated(int bossId)
    {
        sessionPlayerMetrics.LevelsBeaten++;
    }

    public void SetPlayerName(string name)
    {
        CurrentPlayerName = name;
    }

    public void IncreaseScore(int scoreChange)
    {
        sessionPlayerMetrics.Score += scoreChange;
        OnScoreChange?.Invoke(sessionPlayerMetrics.Score);
    }

    public void ResetScore()
    {
        sessionPlayerMetrics.Score = 0;
        OnScoreChange?.Invoke(sessionPlayerMetrics.Score);
    }

    private void OnDestroy()
    {
        EventManager.OnBossDefeated -= OnBossDefeated;
        EventManager.OnPickup1 -= OnPickup1;
        EventManager.OnPickup2 -= OnPickup2;
        EventManager.OnPickup3 -= OnPickup3;
        EventManager.OnAttempt -= OnAttempt;

        PlayerController.OnPlayerJump -= OnPlayerJump;
        PlayerHealth.OnPlayerHealthChanged -= OnPlayerHealthChanged;

        AuthenticationService.Instance.SignedIn -= OnSignedIn;
    }

    public struct PlayerMetrics
    {
        public int Attempts { get; set; }
        public int Score { get; set; }
        public int LevelsBeaten { get; set; }
        public int Jumps { get; set; }
        public int JetpackPickupAmount { get; set; }
        public int LowGravityPickupAmount { get; set; }
        public int HealPickupAmount { get; set; }

        public override readonly string ToString() =>
            $"Attempts: {Attempts}\nScore: {Score}\nLevels Beaten: {LevelsBeaten}\nJumps: {Jumps}\n\n"
            + $"<color=#E94343>Jetpacks: {JetpackPickupAmount}</color>\n<color=#67C5EC>Low Gravity: {LowGravityPickupAmount}</color>\n<color=#67EC81>Heals: {HealPickupAmount}</color>";
    }
}

#region References
/*

Unity. s.a. Unity SDK tutorial, n.d. [Online]. Available at: https://docs.unity.com/ugs/manual/cloud-save/manual/tutorials/unity-sdk#Player_Data [Accessed 01 June 2024]

*/
#endregion
