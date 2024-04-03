using UnityEngine;

public class ObjectHover : MonoBehaviour
{
    [Header("Configs")]
    [Header("Speed")]
    [SerializeField]
    [Range(0.5f, 2f)]
    private float minSpeed = 0.5f;

    [SerializeField]
    [Range(0.5f, 2f)]
    private float maxSpeed = 1.5f;

    [Header("Amplitude")]
    [SerializeField]
    [Range(0.5f, 2f)]
    private float minAmplitude = 0.5f;

    [SerializeField]
    [Range(0.5f, 2f)]
    private float maxAmplitude = 1.5f;

    [Header("Rotation Speed")]
    [SerializeField]
    [Range(10, 60)]
    private int minRotationSpeed = 10;

    [SerializeField]
    [Range(10, 60)]
    private int maxRotationSpeed = 30;
    private float speed;
    private float amplitude;
    private Vector3 originalTransform;
    private Vector3 randomRotation;

    private void Awake()
    {
        originalTransform = transform.position;
        speed = Random.Range(minSpeed, maxSpeed);
        amplitude = Random.Range(minAmplitude, maxAmplitude);

        int[] randoms = new int[3];
        for (int i = 0; i < randoms.Length; i++)
            randoms[i] = Random.Range(minRotationSpeed, maxRotationSpeed);

        randomRotation = new Vector3(randoms[0], randoms[1], randoms[2]);
    }

    private void Update()
    {
        float newX = originalTransform.x + Mathf.Sin(Time.time * speed * 0.5f) * amplitude;
        float newY = originalTransform.y + Mathf.Sin(Time.time * speed) * amplitude;

        transform.position = new Vector3(newX, newY, transform.position.z);
        transform.Rotate(randomRotation * Time.deltaTime);
    }
}
