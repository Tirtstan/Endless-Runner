using System.Collections;
using UnityEngine;

public class UndergroundTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(SceneTransition());
        }
    }

    private IEnumerator SceneTransition()
    {
        yield return new WaitForSeconds(0.25f);
        SceneTransitionManager.Instance.LoadScene(2, SceneTransitionManager.TransitionType.Bottom);
    }
}
