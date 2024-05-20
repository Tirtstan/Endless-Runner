using UnityEngine;

public class ObjectHover : MonoBehaviour
{
    [Header("Configs")]
    [Header("Speed")]
    [SerializeField]
    private Vector2 speedRange = new(0.5f, 1.5f);

    [Header("Amplitude")]
    [SerializeField]
    private Vector2 amplitudeRange = new(0.5f, 1.5f);

    [Header("Rotation Speed")]
    [SerializeField]
    private Vector2 rotationSpeedRange = new(10, 30);
    private float speed;
    private float amplitude;
    private Vector3 originalTransform;
    private Vector3 randomRotation;

    private void Awake()
    {
        originalTransform = transform.position;
        speed = Random.Range(speedRange.x, speedRange.y);
        amplitude = Random.Range(amplitudeRange.x, amplitudeRange.y);

        int[] randoms = new int[3];
        for (int i = 0; i < randoms.Length; i++)
            randoms[i] = Random.Range((int)rotationSpeedRange.x, (int)rotationSpeedRange.y);

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
