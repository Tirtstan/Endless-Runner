using System.Collections;
using UnityEngine;

public class SandwormGround : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField]
    [Range(1, 10)]
    private float lerpDuration = 4;
    private BoxCollider boxCollider;

    private void Awake()
    {
        boxCollider = gameObject.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.enabled = false;
    }

    private void Start()
    {
        EventManager.OnBoss2Attack += OnBoss2Attack;
    }

    private void OnBoss2Attack(float laneId, float duration)
    {
        if ((transform.position.x == -2.5f && laneId == 0) || (transform.position.x == 2.5f && laneId == 1))
        {
            boxCollider.enabled = true;
            gameObject.tag = "Obstacle";
            StartCoroutine(TransitionGround(duration));
        }
    }

    private IEnumerator TransitionGround(float duration)
    {
        float elapsedTime = 0;
        while (elapsedTime < lerpDuration)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                new Vector3(transform.position.x, 4f, transform.position.z),
                elapsedTime
            );
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                new Vector3(transform.localScale.x, 5f, transform.localScale.z),
                elapsedTime
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(duration);
        Reset();

        elapsedTime = 0;
        while (elapsedTime < lerpDuration)
        {
            transform.position = Vector3.Lerp(
                transform.position,
                new Vector3(transform.position.x, 0f, transform.position.z),
                elapsedTime
            );
            transform.localScale = Vector3.Lerp(
                transform.localScale,
                new Vector3(transform.localScale.x, 1f, transform.localScale.z),
                elapsedTime
            );

            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    private void Reset()
    {
        boxCollider.enabled = false;
        gameObject.tag = "Untagged";
    }

    private void OnDestroy()
    {
        EventManager.OnBoss2Attack -= OnBoss2Attack;
    }
}
