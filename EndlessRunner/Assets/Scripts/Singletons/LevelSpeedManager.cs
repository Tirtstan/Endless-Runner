using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSpeedManager : MonoBehaviour
{
    public static LevelSpeedManager Instance { get; private set; }

    [Header("Configs")]
    [SerializeField]
    [Range(0.01f, 0.1f)]
    private float speedIncreaseRate = 0.05f;

    [SerializeField]
    [Range(20, 25)]
    private float levelSpeedCap = 22.5f;
    public float CurrentLevelSpeed = 10f;

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

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.buildIndex == 0)
            Destroy(gameObject);
    }

    private void Update()
    {
        if (CurrentLevelSpeed >= levelSpeedCap)
            return;

        CurrentLevelSpeed += Time.deltaTime * speedIncreaseRate;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
