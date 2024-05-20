using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.Instance.SpawnObstacle();
            LevelManager.Instance.DestroyObstacle(transform.parent.gameObject);
        }
    }
}
