using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    public static event System.Action<int> OnPlayerHit;
    private Rigidbody rb;
    private CameraShake cameraShake;

    [Header("Configs")]
    [SerializeField]
    private float hitForce = 8f;

    [SerializeField]
    private float shakeDuration = 0.15f;

    [field: SerializeField]
    public int MaxHealth { get; set; } = 3;
    public int CurrentHealth { get; set; }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
        CurrentHealth = MaxHealth;
    }

    public void Death() { }

    public void Heal(int healAmount)
    {
        CurrentHealth += healAmount;
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;
        OnPlayerHit?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
            Death();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            cameraShake.Shake(shakeDuration);
            rb.AddForce(Vector3.up * hitForce, ForceMode.Impulse);
            TakeDamage(1);
        }
    }
}
