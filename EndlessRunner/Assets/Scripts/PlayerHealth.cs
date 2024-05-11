using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    public static event System.Action<int> OnPlayerHealth;
    private Rigidbody rb;
    private Animator animator;
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
        animator = GetComponent<Animator>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
        CurrentHealth = MaxHealth;
    }

    public void Death()
    {
        animator.updateMode = AnimatorUpdateMode.UnscaledTime; // plays animation even when death screen is up
        animator.SetTrigger("Death");
    }

    public void Heal(int healAmount)
    {
        CurrentHealth += healAmount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        OnPlayerHealth?.Invoke(CurrentHealth);
    }

    public void TakeDamage(int damage)
    {
        float amount = CurrentHealth >= 3 ? 0.1f : 0.15f;
        cameraShake.Shake(shakeDuration, amount);
        rb.AddForce(Vector3.up * hitForce, ForceMode.Impulse);

        CurrentHealth -= damage;
        OnPlayerHealth?.Invoke(CurrentHealth);

        if (CurrentHealth <= 0)
            Death();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            TakeDamage(1);
        }
    }
}
