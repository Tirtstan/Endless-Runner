using System;
using System.Collections;
using UnityEngine;

public class PickupManager : MonoBehaviour
{
    public static PickupManager Instance { get; private set; }

    [Header("Configs")]
    [Header("Jetpack")]
    [SerializeField]
    private float jetpackTime = 8f;

    [SerializeField]
    private float yPos = 10f;

    [SerializeField]
    private float upwardSpeed = 40f;
    public static event Action<float> OnJetpackTime;
    private readonly WaitForSeconds waitForOneSec = new(1);
    private Rigidbody playerRb;
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
            return;
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
        switch (pickupType)
        {
            default:
            case ItemPickup.Type.Jetpack:
                StartCoroutine(StartJetpack());
                break;
            case ItemPickup.Type.LowGravity:
                break;
            case ItemPickup.Type.SpeedBoots:
                break;
        }
    }

    private IEnumerator StartJetpack() // add particles
    {
        PlayerController playerController = playerRb.gameObject.GetComponent<PlayerController>();
        playerController.ToggleGravity(false);
        isJetpackActive = true;

        for (int i = 0; i < jetpackTime; i++)
        {
            OnJetpackTime?.Invoke(jetpackTime - i);
            yield return waitForOneSec;
        }
        OnJetpackTime?.Invoke(0);

        isJetpackActive = false;
        playerController.ToggleGravity(true);
    }
}
