using UnityEngine;

public class UFO : MonoBehaviour
{
    [Header("UFO Configs")]
    [SerializeField]
    private float speed = 5f;

    [SerializeField]
    private float yOffset = 5f;

    [SerializeField]
    private float zOffset = 30f;
    private GameObject player;
    private float xTarget = 0;

    private void Start()
    {
        player = GameManager.Instance.GetPlayer();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector3(xTarget, yOffset, player.transform.position.z + zOffset),
            speed * Time.deltaTime
        );
    }
}
