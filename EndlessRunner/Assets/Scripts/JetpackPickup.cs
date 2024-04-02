using System.Collections;
using UnityEngine;

public class JetpackPickup : ItemPickup
{
    [Header("Configs")]
    [SerializeField]
    private float jetpackTime = 8f;

    [SerializeField]
    private float yOffset = 10f;
    private Collider playerCollider;
    private Vector3 originalPosition;
    private WaitForSeconds waitForSeconds;

    private void Awake()
    {
        waitForSeconds = new WaitForSeconds(1f);
    }

    protected override void OnPickup(Collider collider)
    {
        playerCollider = collider;
        originalPosition = playerCollider.transform.position;
        StartCoroutine(StartJetpack());
    }

    private IEnumerator StartJetpack()
    {
        Debug.Log("Jetpack activated");

        PlayerController playerController = playerCollider.GetComponent<PlayerController>();
        playerController.SwitchGravity(false);
        playerCollider.transform.position = new Vector3(
            originalPosition.x,
            originalPosition.y + yOffset,
            originalPosition.z
        );

        for (int i = 0; i < jetpackTime; i++)
        {
            Debug.Log($"Jetpack time left: {jetpackTime - i}");
            yield return waitForSeconds;
        }
        Debug.Log("Jetpack deactivated");

        playerController.SwitchGravity(true);
        Destroy(gameObject);
    }
}
