using System.Collections;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public static PickupManager Instance { get; private set; }

    [Header("Components")]
    [SerializeField]
    private GameObject[] pickupPrefabs;

    [Header("Configs")]
    public float SpawnPercentage = 20f;

    [Header("Jetpack")]
    [SerializeField]
    private float jetpackTime = 8f;

    [SerializeField]
    private float yPos = 10f;

    [SerializeField]
    private float upwardSpeed = 40f;

    [Header("Low Gravity")]
    [SerializeField]
    private float lowGravityTime = 8f;

    [SerializeField]
    private float gravityScale = 1f;

    [Header("Speed Boots")]
    [SerializeField]
    private int healAmount = 1;
    public static event System.Action<ItemPickup.Type, float> OnPickupTime;
    private readonly WaitForSeconds waitForOneSec = new(1);
    private Rigidbody playerRb;
    private PlayerController playerController;
    private ItemPickup.Type currentPickupType;
    private bool isJetpackActive;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (isJetpackActive)
        {
            playerRb.position = Vector3.MoveTowards(
                playerRb.position,
                new Vector3(playerRb.position.x, yPos, playerRb.position.z),
                upwardSpeed * Time.fixedDeltaTime
            );
        }
    }

    public void ActivatePickup(Collider other, ItemPickup.Type pickupType)
    {
        playerRb = other.gameObject.GetComponent<Rigidbody>();
        playerController = other.gameObject.GetComponent<PlayerController>();
        currentPickupType = pickupType;

        switch (pickupType)
        {
            default:
            case ItemPickup.Type.Jetpack:
                ResetPickupEffects();
                StartCoroutine(StartJetpack());
                break;
            case ItemPickup.Type.LowGravity:
                ResetPickupEffects();
                StartCoroutine(StartLowGravity());
                break;
            case ItemPickup.Type.Heal:
                Heal();
                break;
        }
    }

    private IEnumerator StartJetpack()
    {
        playerController.ToggleGravity(false);
        isJetpackActive = true;

        for (int i = 0; i < jetpackTime; i++)
        {
            OnPickupTime?.Invoke(currentPickupType, jetpackTime - i);
            yield return waitForOneSec;
        }
        OnPickupTime?.Invoke(currentPickupType, 0);

        isJetpackActive = false;
        ResetPickupEffects();
    }

    private IEnumerator StartLowGravity()
    {
        AudioManager.Instance.ToggleLowPassFilter(true);
        playerController.ChangeGravity(gravityScale);

        for (int i = 0; i < lowGravityTime; i++)
        {
            OnPickupTime?.Invoke(currentPickupType, lowGravityTime - i);
            yield return waitForOneSec;
        }
        OnPickupTime?.Invoke(currentPickupType, 0);

        ResetPickupEffects();
    }

    private void Heal()
    {
        IDamagable damagable = playerRb.gameObject.GetComponent<IDamagable>();
        damagable.Heal(healAmount);
    }

    private void ResetPickupEffects()
    {
        StopAllCoroutines();
        playerController.ResetGravityMultiplier();
        playerController.ToggleGravity(true);
        AudioManager.Instance.ToggleLowPassFilter(false);
    }

    public GameObject GetRandomPickup() => pickupPrefabs[Random.Range(0, pickupPrefabs.Length)];

    private void OnDestroy()
    {
        ResetPickupEffects();
    }
}
