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
    private float yOffset = 10f;

    [SerializeField]
    private float upwardSpeed = 40f;
    public static event Action<float> OnJetpackTime;
    private Vector3 originalPosition;
    private WaitForSeconds waitForSeconds;
    private Collider playerCollider;
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

        waitForSeconds = new WaitForSeconds(1);
    }

    private void Update()
    {
        if (isJetpackActive)
        {
            playerCollider.transform.position = Vector3.MoveTowards(
                playerCollider.transform.position,
                new Vector3(
                    playerCollider.transform.position.x,
                    originalPosition.y + yOffset,
                    playerCollider.transform.position.z
                ),
                upwardSpeed * Time.deltaTime
            );
        }
    }

    public void ActivateJetpack(Collider other)
    {
        playerCollider = other;
        originalPosition = playerCollider.transform.position;
        StartCoroutine(StartJetpack());
    }

    private IEnumerator StartJetpack() // add particles
    {
        PlayerController playerController = playerCollider.GetComponent<PlayerController>();
        playerController.SwitchGravity(false);
        isJetpackActive = true;

        for (int i = 0; i < jetpackTime; i++)
        {
            OnJetpackTime?.Invoke(jetpackTime - i);
            yield return waitForSeconds;
        }
        OnJetpackTime?.Invoke(0);

        isJetpackActive = false;
        playerController.SwitchGravity(true);
    }
}
