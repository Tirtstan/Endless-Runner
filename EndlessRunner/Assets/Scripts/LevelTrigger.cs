using UnityEngine;

public class LevelTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject parent;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            LevelManager.Instance.SpawnObstacle(new Vector3(0, 0, 100));
            Destroy(parent, 3);
        }
    }
}
