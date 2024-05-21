using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField]
    private GameObject playerPrefab;
    private GameObject player;

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

        QualitySettings.vSyncCount = 1;

        if (SceneManager.GetActiveScene().buildIndex != 0 && player == null)
            player = Instantiate(playerPrefab, Vector3.forward, Quaternion.identity);
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
