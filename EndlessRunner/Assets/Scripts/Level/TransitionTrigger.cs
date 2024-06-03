using System.Collections;
using UnityEngine;

public class TransitionTrigger : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField]
    [Range(0, 1)]
    private float transitionTime = 0.25f;

    [SerializeField]
    private int buildIndex = 1;

    [SerializeField]
    private SceneTransitionManager.TransitionType transitionType = SceneTransitionManager.TransitionType.Top;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SceneTransition());
            other.gameObject.GetComponent<PlayerController>().ToggleCollider(false);
        }
    }

    private IEnumerator SceneTransition()
    {
        yield return new WaitForSeconds(transitionTime);
        SceneTransitionManager.Instance.LoadScene(buildIndex, transitionType);
    }
}
