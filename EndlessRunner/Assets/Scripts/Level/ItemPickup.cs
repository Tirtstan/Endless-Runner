using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public enum Type
    {
        Jetpack = 0,
        LowGravity = 1,
        Heal = 2
    }

    [Header("Components")]
    [SerializeField]
    private AudioClip pickupClip;

    [Header("Config")]
    [SerializeField]
    private Type pickupType;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
            PickupManager.Instance.ActivatePickup(other, pickupType);
            switch (pickupType)
            {
                case Type.Jetpack:
                    EventManager.Instance.InvokePickup1();
                    break;
                case Type.LowGravity:
                    EventManager.Instance.InvokePickup2();
                    break;
                case Type.Heal:
                    EventManager.Instance.InvokePickup3();
                    break;
            }
            Destroy(gameObject);
        }
    }
}
