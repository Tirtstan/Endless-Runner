using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public enum TransitionType
    {
        Top = 0,
        Bottom = 1
    }

    public static SceneTransitionManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField]
    private GameObject loaderCanvas;

    [SerializeField]
    private Image image;

    [Header("Configs")]
    [SerializeField]
    private AnimationCurve transitionCurve;

    [SerializeField]
    [Range(1, 5)]
    private float transitionTime = 1.5f;

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

    public async void LoadScene(int buildIndex, TransitionType transitionType)
    {
        image.fillAmount = 0;
        var scene = SceneManager.LoadSceneAsync(buildIndex);
        scene.allowSceneActivation = false;
        loaderCanvas.SetActive(true);

        switch (transitionType)
        {
            case TransitionType.Top:
                image.fillOrigin = 0;
                break;
            case TransitionType.Bottom:
                image.fillOrigin = 1;
                break;
        }

        while (image.fillAmount < 1) // TODO
        {
            image.fillAmount += Time.unscaledDeltaTime * transitionTime;
            image.fillAmount = Mathf.Clamp01(image.fillAmount);
            image.fillAmount = transitionCurve.Evaluate(image.fillAmount);
            await Task.Yield();
        }

        scene.allowSceneActivation = true;

        while (image.fillAmount > 0)
        {
            image.fillAmount -= Time.unscaledDeltaTime * transitionTime;
            image.fillAmount = Mathf.Clamp01(image.fillAmount);
            image.fillAmount = transitionCurve.Evaluate(image.fillAmount);
            await Task.Yield();
        }

        loaderCanvas.SetActive(false);
    }
}
