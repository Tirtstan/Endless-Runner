using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PickupManager.Instance.ActivateJetpack(other);
            Destroy(gameObject);
        }
    }
}
