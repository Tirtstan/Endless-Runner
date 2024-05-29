using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [Header("Components")]
    [Header("Menus")]
    [SerializeField]
    private GameObject mainPanel;

    [SerializeField]
    private GameObject statsPanel;

    [Header("Buttons")]
    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button statsButton;

    [SerializeField]
    private Button exitButton;

    [SerializeField]
    private Button backButton;

    [Header("Configs")]
    [SerializeField]
    private float mainPanelYRot = -12;

    [SerializeField]
    private float statsPanelYRot = 25;

    [SerializeField]
    [Range(1, 10)]
    private float rotSpeed = 4;
    private Camera mainCamera;
    private float targetYRot = -12;

    [SerializeField]
    private void Awake()
    {
        mainCamera = Camera.main;

        playButton.onClick.AddListener(OnPlayClick);
        statsButton.onClick.AddListener(OnStatsClick);
        exitButton.onClick.AddListener(OnExitClick);
        backButton.onClick.AddListener(OnBackClick);

        OnBackClick();
    }

    private void Update()
    {
        mainCamera.transform.rotation = Quaternion.Slerp(
            mainCamera.transform.rotation,
            Quaternion.Euler(
                mainCamera.transform.eulerAngles.x,
                targetYRot,
                mainCamera.transform.eulerAngles.z
            ),
            Time.deltaTime * rotSpeed
        );
    }

    private void OnPlayClick() => SceneManager.LoadSceneAsync(1);

    private void OnStatsClick()
    {
        targetYRot = statsPanelYRot;
    }

    private void OnExitClick() => Application.Quit();

    private void OnBackClick()
    {
        targetYRot = mainPanelYRot;
    }
}
