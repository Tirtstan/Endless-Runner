using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudSave;
using UnityEngine;

public class PlayerMetricsManager : MonoBehaviour
{
    public static PlayerMetricsManager Instance { get; private set; }
    public static event Action<int> OnScoreChange;
    public string CurrentPlayerName { get; private set; } = "Player";
    private PlayerMetrics totalPlayerMetrics = new();
    private PlayerMetrics sessionPlayerMetrics = new();

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
    }

    public string GetSessionPlayerMetrics() => sessionPlayerMetrics.ToString();

    public string GetTotalPlayerMetrics() => totalPlayerMetrics.ToString();

    private void Start()
    {
        EventManager.OnBossDefeated += OnBossDefeated;
        EventManager.OnPickup1 += OnPickup1;
        EventManager.OnPickup2 += OnPickup2;
        EventManager.OnPickup3 += OnPickup3;

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

        totalPlayerMetrics.Score += sessionPlayerMetrics.Score;
        totalPlayerMetrics.LevelsBeaten += sessionPlayerMetrics.LevelsBeaten;
        totalPlayerMetrics.Jumps += sessionPlayerMetrics.Jumps;
        totalPlayerMetrics.JetpackPickupAmount += sessionPlayerMetrics.JetpackPickupAmount;
        totalPlayerMetrics.LowGravityPickupAmount += sessionPlayerMetrics.LowGravityPickupAmount;
        totalPlayerMetrics.HealPickupAmount += sessionPlayerMetrics.HealPickupAmount;

        var playerData = new Dictionary<string, object>
        {
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
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to save player data!: {e.Message}");
        }
    }

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

            if (playerData.TryGetValue("Score", out var score))
                totalPlayerMetrics.Score = score.Value.GetAs<int>();

            if (playerData.TryGetValue("LevelsBeaten", out var levelsBeaten))
                totalPlayerMetrics.LevelsBeaten = levelsBeaten.Value.GetAs<int>();

            if (playerData.TryGetValue("Jumps", out var jumps))
                totalPlayerMetrics.Jumps = jumps.Value.GetAs<int>();

            if (playerData.TryGetValue("JetpackPickupAmount", out var jetpackPickupAmount))
                totalPlayerMetrics.JetpackPickupAmount = jetpackPickupAmount.Value.GetAs<int>();

            if (playerData.TryGetValue("LowGravityPickupAmount", out var lowGravityPickupAmount))
                totalPlayerMetrics.LowGravityPickupAmount =
                    lowGravityPickupAmount.Value.GetAs<int>();

            if (playerData.TryGetValue("HealPickupAmount", out var healPickupAmount))
                totalPlayerMetrics.HealPickupAmount = healPickupAmount.Value.GetAs<int>();
        }
        catch (Exception e)
        {
            Debug.LogWarning($"Failed to load player data!: {e.Message}");
        }
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

    private void OnDestroy()
    {
        EventManager.OnBossDefeated -= OnBossDefeated;
        EventManager.OnPickup1 -= OnPickup1;
        EventManager.OnPickup2 -= OnPickup2;
        EventManager.OnPickup3 -= OnPickup3;

        PlayerController.OnPlayerJump -= OnPlayerJump;
        PlayerHealth.OnPlayerHealthChanged -= OnPlayerHealthChanged;

        AuthenticationService.Instance.SignedIn -= OnSignedIn;
    }

    public struct PlayerMetrics
    {
        public int Score { get; set; }
        public int LevelsBeaten { get; set; }
        public int Jumps { get; set; }
        public int JetpackPickupAmount { get; set; }
        public int LowGravityPickupAmount { get; set; }
        public int HealPickupAmount { get; set; }

        public override readonly string ToString() =>
            $"Score: {Score}\nLevels Beaten: {LevelsBeaten}\nJumps: {Jumps}\n"
            + $"<color=#E94343>Jetpacks: {JetpackPickupAmount}\n<color=#67C5EC>Low Gravity: {LowGravityPickupAmount}\n<color=#67EC81>Heals: {HealPickupAmount}</color>";
    }
}
