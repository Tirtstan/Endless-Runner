using System.Collections;
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
    [Range(0, 2)]
    private float transitionTime = 0.75f;
    private readonly WaitForSecondsRealtime wait = new(0.5f);

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

    public void LoadScene(int buildIndex, TransitionType transitionType)
    {
        StartCoroutine(Transition(buildIndex, transitionType));
    }

    private IEnumerator Transition(int buildIndex, TransitionType transitionType)
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

        float elapsedTime = 0;
        while (elapsedTime < transitionTime)
        {
            image.fillAmount = transitionCurve.Evaluate(elapsedTime / transitionTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        image.fillAmount = 1;

        scene.allowSceneActivation = true;
        yield return wait; // prevents new scene freezing

        elapsedTime = 0;
        while (elapsedTime < transitionTime)
        {
            image.fillAmount = 1 - transitionCurve.Evaluate(elapsedTime / transitionTime);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        image.fillAmount = 0;

        loaderCanvas.SetActive(false);
    }
}
