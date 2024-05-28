using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public static event Action<int> OnStartTime;

    [Header("Components")]
    [SerializeField]
    private GameObject playerPrefab;

    [Header("Config")]
    [SerializeField]
    private int startTime = 3;
    private GameObject player;
    private readonly WaitForSecondsRealtime waitForOneSec = new(1);

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

        QualitySettings.vSyncCount = 1;
        player = Instantiate(playerPrefab, Vector3.forward, Quaternion.identity);
    }

    private void Start()
    {
        StartTimer();
    }

    public void StartTimer()
    {
        StartCoroutine(StartCountdown());
    }

    private IEnumerator StartCountdown()
    {
        Time.timeScale = 0;
        OnStartTime?.Invoke(startTime);
        for (int i = 0; i < startTime; i++)
        {
            OnStartTime?.Invoke(startTime - i);
            yield return waitForOneSec;
        }
        OnStartTime?.Invoke(0);
        Time.timeScale = 1;
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public GameObject GetPlayer()
    {
        return player;
    }
}
