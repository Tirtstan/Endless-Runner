using System.Collections;
using UnityEngine;

public class Sandworm : MonoBehaviour
{
    [Header("Configs")]
    [Header("Timings")]
    [SerializeField]
    private float startCooldown = 5f;

    [SerializeField]
    private Vector2 endTimeRange = new(35, 50);
    private GameObject player;
    private CameraShake cameraShake;
    private AudioSource audioSource;
    private Coroutine attackCoroutine;

    private void Awake()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        player = GameManager.Instance.GetPlayer();
        EventManager.Instance.InvokeBoss2Spawned();
        StartCoroutine(StartCooldown());
    }

    private IEnumerator StartCooldown()
    {
        cameraShake.Shake(startCooldown, 0.05f);
        float elapsedTime = 0;
        while (elapsedTime < startCooldown)
        {
            RenderSettings.fogEndDistance = Mathf.Lerp(
                RenderSettings.fogEndDistance,
                RenderSettings.fogStartDistance * 1.2f,
                Time.deltaTime * 0.4f
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        AudioManager.Instance.PlayBoss2Music();

        // attackCoroutine = StartCoroutine(Attack());
        StartCoroutine(End());
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            yield return null;
        }
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(Random.Range(endTimeRange.x, endTimeRange.y));
        StopCoroutine(attackCoroutine);
        ResetAttacks();

        EventManager.Instance.InvokeBossDefeated(2);
        Destroy(gameObject, 3f);
    }

    private void ResetAttacks() { }
}
