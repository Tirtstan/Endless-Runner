using System.Collections;
using UnityEngine;

public class PlayerHealth : MonoBehaviour, IDamagable
{
    public static event System.Action<int> OnPlayerHealthChanged;
    private Rigidbody rb;
    private Animator animator;
    private AudioSource audioSource;
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
        audioSource = GetComponent<AudioSource>();
        cameraShake = Camera.main.GetComponent<CameraShake>();
        CurrentHealth = MaxHealth;
    }

    private void Update()
    {
        if (transform.position.y < -20) // if player falls off the map
            TakeDamage(99);
    }

    public void Death()
    {
        StartCoroutine(AnimateDeath());
        animator.SetTrigger("Death");
    }

    public void Heal(int healAmount)
    {
        CurrentHealth += healAmount;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        OnPlayerHealthChanged?.Invoke(CurrentHealth);
    }

    public void TakeDamage(int damage)
    {
        float amount = CurrentHealth >= 3 ? 0.1f : 0.15f;
        cameraShake.Shake(shakeDuration, amount);
        rb.AddForce(Vector3.up * hitForce, ForceMode.Impulse);

        CurrentHealth -= damage;
        CurrentHealth = Mathf.Clamp(CurrentHealth, 0, MaxHealth);
        OnPlayerHealthChanged?.Invoke(CurrentHealth);

        audioSource.Play();

        if (CurrentHealth <= 0)
            Death();
    }

    private IEnumerator AnimateDeath() // plays animation even when death screen is up
    {
        animator.updateMode = AnimatorUpdateMode.UnscaledTime;
        yield return new WaitForSecondsRealtime(1.5f);
        animator.updateMode = AnimatorUpdateMode.Normal;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
            TakeDamage(1);
    }
}
