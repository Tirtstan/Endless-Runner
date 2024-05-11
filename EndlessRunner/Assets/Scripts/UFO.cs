using System.Collections;
using UnityEngine;

public class UFO : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private GameObject areaPrefab;

    [Header("UFO Configs")]
    [SerializeField]
    private float speed = 1f;

    [SerializeField]
    private float yOffset = 5f;

    [SerializeField]
    private float zOffset = 30f;

    [SerializeField]
    private float startCooldown = 15;

    [SerializeField]
    private float attackStartUp = 6;
    private GameObject player;
    private CameraShake cameraShake;
    private float xTarget = 0;
    private Coroutine moveCoroutine;
    private Coroutine attackCoroutine;
    private GameObject area;

    private void Awake()
    {
        cameraShake = Camera.main.GetComponent<CameraShake>();
    }

    private void Start()
    {
        player = GameManager.Instance.GetPlayer();
        StartCoroutine(StartCooldown());
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            new Vector3(xTarget, yOffset, player.transform.position.z + zOffset),
            speed * Time.deltaTime
        );
    }

    private IEnumerator StartCooldown()
    {
        yield return new WaitForSeconds(startCooldown);
        moveCoroutine = StartCoroutine(Move());
        attackCoroutine = StartCoroutine(Attack());
        StartCoroutine(End());
    }

    private IEnumerator Move()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(7, 15));
            xTarget = RandomLane();
        }
    }

    private IEnumerator Attack()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(4, 15));

            float xPos = Random.Range(0, 2) == 0 ? RandomLane() : player.transform.position.x;
            Vector3 areaPos = new(xPos, 0.01f, player.transform.position.z);
            area = Instantiate(areaPrefab, areaPos, Quaternion.identity);

            yield return new WaitForSeconds(attackStartUp);

            if (
                Mathf.Approximately(player.transform.position.x, areaPos.x)
                && player.transform.position.y < 2.5f
            )
            {
                player.GetComponent<IDamagable>().TakeDamage(1);
            }
            else
            {
                cameraShake.Shake(0.15f);
            }

            Destroy(area);
        }
    }

    private IEnumerator End()
    {
        yield return new WaitForSeconds(Random.Range(40, 80));
        StopCoroutine(moveCoroutine);
        StopCoroutine(attackCoroutine);
        Destroy(area);

        xTarget = 80;
        Destroy(gameObject, 5f);
    }

    private float RandomLane()
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
}
