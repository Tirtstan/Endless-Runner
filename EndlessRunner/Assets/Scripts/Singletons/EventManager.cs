using System;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static EventManager Instance { get; private set; }
    public static event Action OnPassObstacle;
    public static event Action OnBoss1Spawned; // ufo
    public static event Action OnBoss2Spawned; // sand worm
    public static event Action OnPickup1;
    public static event Action OnPickup2;
    public static event Action OnPickup3;
    public static event Action<int> OnBossDefeated;

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

    public void InvokePassObstacle() => OnPassObstacle?.Invoke();

    public void InvokeBoss1Spawned() => OnBoss1Spawned?.Invoke();

    public void InvokeBoss2Spawned() => OnBoss2Spawned?.Invoke();

    public void InvokePickup1() => OnPickup1?.Invoke();

    public void InvokePickup2() => OnPickup2?.Invoke();

    public void InvokePickup3() => OnPickup3?.Invoke();

    public void InvokeBossDefeated(int bossId) => OnBossDefeated?.Invoke(bossId);
}
