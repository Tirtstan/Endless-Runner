using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    public static event Action OnPassObstacle;
    public static event Action<int> OnScoreChange;
    public static event Action OnBoss1Spawned; // ufo
    public static event Action OnBoss2Spawned; // sand worm
    public static event Action<int> OnBossDefeated;
    private int score;

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
        }
    }

    public void InvokePassObstacle()
    {
        OnPassObstacle?.Invoke();
    }

    public void InvokeBoss1Spawned()
    {
        OnBoss1Spawned?.Invoke();
    }

    public void InvokeBoss2Spawned()
    {
        OnBoss2Spawned?.Invoke();
    }

    public void InvokeBossDefeated(int bossId)
    {
        OnBossDefeated?.Invoke(bossId);
    }

    public void IncreaseScore(int value)
    {
        score += value;
        OnScoreChange?.Invoke(score);
    }

    public int GetScore()
    {
        return score;
    }
}
