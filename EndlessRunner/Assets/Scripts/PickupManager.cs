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
    public static event System.Action<float> OnPickupTime;
    private readonly WaitForSeconds waitForOneSec = new(1);
    private Rigidbody playerRb;
    private PlayerController playerController;
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
        switch (pickupType)
        {
            default:
            case ItemPickup.Type.Jetpack:
                StartCoroutine(StartJetpack());
                break;
            case ItemPickup.Type.LowGravity:
                StartCoroutine(StartLowGravity());
                break;
            case ItemPickup.Type.SpeedBoots:
                SpeedBoots();
                break;
        }
    }

    private IEnumerator StartJetpack()
    {
        playerController.ToggleGravity(false);
        isJetpackActive = true;

        for (int i = 0; i < jetpackTime; i++)
        {
            OnPickupTime?.Invoke(jetpackTime - i);
            yield return waitForOneSec;
        }
        OnPickupTime?.Invoke(0);

        isJetpackActive = false;
        playerController.ToggleGravity(true);
    }

    private IEnumerator StartLowGravity()
    {
        playerController.ChangeGravity(gravityScale);

        for (int i = 0; i < lowGravityTime; i++)
        {
            OnPickupTime?.Invoke(lowGravityTime - i);
            yield return waitForOneSec;
        }
        OnPickupTime?.Invoke(0);

        playerController.ResetGravityMultiplier();
    }

    private void SpeedBoots()
    {
        IDamagable damagable = playerRb.gameObject.GetComponent<IDamagable>();
        damagable.Heal(healAmount);
    }

    public GameObject GetRandomPickup()
    {
        return pickupPrefabs[Random.Range(0, pickupPrefabs.Length)];
    }
}
