using UnityEngine;

public abstract class ItemPickup : MonoBehaviour
{
    protected abstract void OnPickup(Collider collider);

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            OnPickup(other);
        }
    }
}
