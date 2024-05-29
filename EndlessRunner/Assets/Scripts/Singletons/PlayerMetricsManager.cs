using System;
using UnityEngine;

public class PlayerMetricsManager : MonoBehaviour
{
    public static PlayerMetricsManager Instance { get; private set; }
    public static event Action<int> OnScoreChange;
    public string CurrentPlayerName { get; private set; } = "Player";
    public int Score { get; private set; }
    public int LevelsBeaten { get; private set; }
    public int Jumps { get; private set; }
    public int JetpackPickupAmount { get; private set; }
    public int LowGravityPickupAmount { get; private set; }
    public int HealPickupAmount { get; private set; }

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

    public string GetPlayerMetrics() =>
        $"Player: {CurrentPlayerName}\nScore: {Score}\nLevels Beaten: {LevelsBeaten}\nJumps: {Jumps}\n"
        + $"<color=#E94343>Jetpacks: {JetpackPickupAmount}\n<color=#67C5EC>Low Gravity: {LowGravityPickupAmount}\n<color=#67EC81>Heals: {HealPickupAmount}</color>";

    private void Start()
    {
        EventManager.OnBossDefeated += OnBossDefeated;
        PlayerController.OnPlayerJump += OnPlayerJump;

        EventManager.OnPickup1 += OnPickup1;
        EventManager.OnPickup2 += OnPickup2;
        EventManager.OnPickup3 += OnPickup3;
    }

    private void OnPickup1()
    {
        JetpackPickupAmount++;
    }

    private void OnPickup2()
    {
        LowGravityPickupAmount++;
    }

    private void OnPickup3()
    {
        HealPickupAmount++;
    }

    private void OnPlayerJump()
    {
        Jumps++;
    }

    private void OnBossDefeated(int bossId)
    {
        LevelsBeaten++;
    }

    public void SetPlayerName(string name)
    {
        CurrentPlayerName = name;
    }

    public void IncreaseScore(int scoreChange)
    {
        Score += scoreChange;
        OnScoreChange?.Invoke(Score);
    }

    private void OnDestroy()
    {
        EventManager.OnBossDefeated -= OnBossDefeated;
        PlayerController.OnPlayerJump -= OnPlayerJump;

        EventManager.OnPickup1 -= OnPickup1;
        EventManager.OnPickup2 -= OnPickup2;
        EventManager.OnPickup3 -= OnPickup3;
    }
}
