using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Transform target;

    [Header("Configs")]
    [Range(0f, 30f)]
    [SerializeField]
    private float zOffset = 10f;

    private void LateUpdate()
    {
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            target.position.z - zOffset
        );
    }
}
