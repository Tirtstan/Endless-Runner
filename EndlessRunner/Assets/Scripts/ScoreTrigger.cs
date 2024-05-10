using System;
using UnityEngine;

public class ScoreTrigger : MonoBehaviour
{
    public static event Action OnObstaclePass;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.IncreaseScore(1);
            OnObstaclePass?.Invoke();
        }
    }
}
