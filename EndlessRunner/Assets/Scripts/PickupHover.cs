using UnityEngine;

public class PickupHover : MonoBehaviour
{
    [Header("Configs")]
    [SerializeField]
    [Range(0, 2f)]
    private float speed = 1;

    [SerializeField]
    [Range(0, 2f)]
    private float amplitude = 1;
    private Vector3 originalTransform;

    private void Awake()
    {
        originalTransform = transform.position;
    }

    private void Update()
    {
        float newY = originalTransform.y + Mathf.Sin(Time.time * speed) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
}
