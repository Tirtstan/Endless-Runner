using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EventManager.Instance.InvokePassObstacle();
            PlayerMetricsManager.Instance.IncreaseScore(1);
        }
    }
}
