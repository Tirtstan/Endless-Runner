using UnityEngine;

public class ObjectHover : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField]
    [Range(0.5f, 2f)]
    private float minSpeed = 0.5f;

    [SerializeField]
    [Range(0.5f, 2f)]
    private float maxSpeed = 1.5f;

    [SerializeField]
    [Range(10, 60)]
    private int minRotationSpeed = 10;

    [SerializeField]
    [Range(10, 60)]
    private int maxRotationSpeed = 60;
    private float speed;
    private float amplitude;
    private Vector3 originalTransform;
    private Vector3 randomRotation;

    private void Awake()
    {
        originalTransform = transform.position;
        speed = Random.Range(minSpeed, maxSpeed);
        amplitude = Random.Range(0.5f, 1.5f);

        int[] randoms = new int[3];
        for (int i = 0; i < randoms.Length; i++)
            randoms[i] = Random.Range(minRotationSpeed, maxRotationSpeed);

        randomRotation = new Vector3(randoms[0], randoms[1], randoms[2]);
    }

    private void Update()
    {
        float newY = originalTransform.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);

        Vector3 rotation = randomRotation * Time.deltaTime;
        transform.Rotate(rotation);
    }
}
