using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject parent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.Instance.SpawnObstacle();
            Destroy(parent, 3);
        }
    }
}
