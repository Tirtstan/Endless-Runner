using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    [Header("Configs")]
    [Range(0f, 30f)]
    [SerializeField]
    private float zOffset = 10f;
    private GameObject player;

    private void Start()
    {
        player = GameManager.Instance.GetPlayer();
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(
            transform.position.x,
            transform.position.y,
            player.transform.position.z - zOffset
        );
    }
}
