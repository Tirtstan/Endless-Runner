using System.Collections;
using UnityEngine;

public class UFO : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private GameObject areaPrefab;

    [SerializeField]
    private LineRenderer[] lineRenderers;

    [Header("Audio")]
    [SerializeField]
    private AudioClip[] laserAttackClips;

    [Header("UFO Configs")]
    [Header("Pacing & Position")]
    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private float yPosition = 5f;

    [SerializeField]
    private float zOffset = 30f;

    [Header("Timings")]
    [SerializeField]
    [Range(5, 15)]
    private float startCooldown = 8;

    [SerializeField]
    [Range(1, 8)]
    private float attackStartUp = 4;

    [SerializeField]
    private Vector2 moveTimeRange = new(3, 7);

    [SerializeField]
    private Vector2 attackTimeRange = new(4, 9);

    [SerializeField]
    private Vector2 endTimeRange = new(40, 60);

    [SerializeField]
    private float bossDeathCooldown = 8f;
    private GameObject player;
    private CameraShake cameraShake;
    private AudioSource audioSource;
    private float xTarget = 0;
    private Coroutine moveCoroutine;
    private Coroutine attackCoroutine;
    private GameObject[] areaAttacks;

    private void Awake()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        player = GameManager.Instance.GetPlayer();
        EventManager.Instance.InvokeBoss1Spawned();
        StartCoroutine(StartCooldown());
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(xTarget, yPosition, player.transform.position.z + zOffset),
            speed * Time.deltaTime
        );
    }

    private IEnumerator StartCooldown()
    {
        AudioManager.Instance.PlayFadeInClip(audioSource, 2f);
        yield return new WaitForSeconds(startCooldown);
        audioSource.Stop();

        moveCoroutine = StartCoroutine(Move());
        attackCoroutine = StartCoroutine(Attack());
        StartCoroutine(End());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(moveTimeRange.x, moveTimeRange.y));
            xTarget = GetRandomLane();
        }
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            areaAttacks = new GameObject[Random.Range(1, 3)];
            float previousLanePos = 1;
            for (int i = 0; i < areaAttacks.Length; i++)
            {
                float lanePos;
                do
                {
                    lanePos = GetRandomLane();
                } while (Mathf.Approximately(previousLanePos, lanePos)); // prevents the areas from spawning on the same lane

                areaAttacks[i] = Instantiate(
                    areaPrefab,
                    new Vector3(lanePos, 0.01f, player.transform.position.z),
                    Quaternion.identity
                );
                previousLanePos = lanePos;
            }

            yield return new WaitForSeconds(attackStartUp);

            for (int i = 0; i < areaAttacks.Length; i++)
            {
                Vector3 parentPos = lineRenderers[i].transform.parent.position;
                lineRenderers[i].SetPosition(0, new Vector3(parentPos.x, parentPos.y + 1f, parentPos.z));
                lineRenderers[i].SetPosition(1, areaAttacks[i].transform.position);

                if (
                    Mathf.Abs(player.transform.position.x - areaAttacks[i].transform.position.x) <= 0.15f
                    && player.transform.position.y < 1.75f
                )
                {
                    player.GetComponent<IDamagable>().TakeDamage(1);
                    break;
                }
                else
                {
                    if (i == areaAttacks.Length - 1) // only shake once if missed
                        cameraShake.Shake(0.15f);
                }
            }

            audioSource.PlayOneShot(laserAttackClips[Random.Range(0, laserAttackClips.Length)]);

            yield return new WaitForSeconds(0.5f);
            ResetAttacks();
            yield return new WaitForSeconds(Random.Range(attackTimeRange.x, attackTimeRange.y));
        }
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(Random.Range(endTimeRange.x, endTimeRange.y));
        StopCoroutine(moveCoroutine);
        StopCoroutine(attackCoroutine);
        ResetAttacks();

        EventManager.Instance.InvokeBossDefeated(1);
        xTarget = 80;
        Destroy(gameObject, bossDeathCooldown);
    }

    private float GetRandomLane()
    {
        int random = Random.Range(0, 3);
        switch (random)
        {
            case 0:
                return -1.5f;
            default:
            case 1:
                return 0;
            case 2:
                return 1.5f;
        }
    }

    private void ResetAttacks()
    {
        foreach (var area in areaAttacks)
            Destroy(area);

        foreach (var lineRenderer in lineRenderers)
        {
            lineRenderer.SetPosition(0, Vector3.zero);
            lineRenderer.SetPosition(1, Vector3.zero);
        }
    }
}
