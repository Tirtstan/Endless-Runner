using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Button playButton;

    [SerializeField]
    private Button exitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(OnPlayClick);
        exitButton.onClick.AddListener(OnExitClick);
    }

    private void OnPlayClick()
    {
        SceneManager.LoadSceneAsync(1);
    }

    private void OnExitClick()
    {
        Application.Quit();
    }
}
