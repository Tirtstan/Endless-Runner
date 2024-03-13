using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.Instance.SpawnObstacle(new Vector3(0, 0, 100));
            Destroy(transform.parent.gameObject, 3);
        }
    }
}
