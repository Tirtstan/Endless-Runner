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
    private Vector3 originalPosition;
    private WaitForSeconds waitForSeconds;

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

    public void ActivateJetpack(Collider other)
    {
        originalPosition = other.transform.position;
        StartCoroutine(StartJetpack(other));
    }

    private IEnumerator StartJetpack(Collider other)
    {
        PlayerController playerController = other.GetComponent<PlayerController>();
        playerController.SwitchGravity(false);
        other.transform.position = new Vector3(
            originalPosition.x,
            originalPosition.y + yOffset,
            originalPosition.z
        );

        for (int i = 0; i < jetpackTime; i++)
        {
            Debug.Log($"Jetpack time left: {jetpackTime - i}");
            yield return waitForSeconds;
        }

        playerController.SwitchGravity(true);
    }
}
