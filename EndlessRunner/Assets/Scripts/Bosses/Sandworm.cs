using System.Collections;
using UnityEngine;

public class Sandworm : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private GameObject areaPrefab;

    [Header("Audio")]
    [SerializeField]
    private AudioClip attackClip;

    [Header("Configs")]
    [Header("Visuals")]
    [SerializeField]
    private float fogStartDistance = 20;

    [SerializeField]
    [Range(0, 2)]
    private float fogSpeed = 0.4f;

    [Header("Timings")]
    [SerializeField]
    private float startCooldown = 5f;

    [SerializeField]
    [Range(1, 8)]
    private float attackStartUp = 4;

    [SerializeField]
    private float attackDuration = 2.5f;

    [SerializeField]
    private Vector2 attackTimeRange = new(3, 8);

    [SerializeField]
    private Vector2 endTimeRange = new(25, 40);

    [SerializeField]
    private float bossDeathCooldown = 8f;
    private GameObject player;
    private CameraShake cameraShake;
    private AudioSource audioSource;
    private Coroutine attackCoroutine;
    private GameObject areaAttack;

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
        AudioManager.Instance.PlayFadeInClip(audioSource, 2f);
        cameraShake.Shake(startCooldown, 0.05f);

        float elapsedTime = 0;
        while (elapsedTime < startCooldown)
        {
            RenderSettings.fogStartDistance = fogStartDistance;
            RenderSettings.fogEndDistance = Mathf.Lerp(
                RenderSettings.fogEndDistance,
                RenderSettings.fogStartDistance,
                Time.deltaTime * fogSpeed
            );
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        audioSource.Stop();

        attackCoroutine = StartCoroutine(Attack());
        StartCoroutine(End());
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            float lanePos = GetRandomLane();
            areaAttack = Instantiate(
                areaPrefab,
                new Vector3(lanePos, 0.01f, player.transform.position.z + 5f),
                Quaternion.identity
            );

            yield return new WaitForSeconds(attackStartUp);
            audioSource.volume = 0.5f;
            audioSource.PlayOneShot(attackClip);
            Destroy(areaAttack);

            EventManager.Instance.InvokeBoss2Attack(lanePos == -1.5f ? 0 : 1, attackDuration); // 0 is left, 1 is right
            yield return new WaitForSeconds(attackDuration);

            ResetAttacks();
            yield return new WaitForSeconds(Random.Range(attackTimeRange.x, attackTimeRange.y));
        }
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(Random.Range(endTimeRange.x, endTimeRange.y));
        StopCoroutine(attackCoroutine);
        ResetAttacks();

        EventManager.Instance.InvokeBossDefeated(2);
        Destroy(gameObject, bossDeathCooldown);
    }

    // excludes the middle lane for balancing
    private float GetRandomLane() => Random.Range(0, 2) == 0 ? -1.5f : 1.5f;

    private void ResetAttacks()
    {
        if (areaAttack != null)
            Destroy(areaAttack);
    }
}
