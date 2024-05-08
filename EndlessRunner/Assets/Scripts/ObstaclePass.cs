using System;
using UnityEngine;

public class ObstaclePass : MonoBehaviour
{
    public static event Action OnObstaclePass;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.Instance.IncreaseScore();
            OnObstaclePass?.Invoke();
        }
    }
}
