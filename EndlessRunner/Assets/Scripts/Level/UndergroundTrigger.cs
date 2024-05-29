using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadSceneAsync(2);
    }
}
