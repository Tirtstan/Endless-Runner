using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Configs")]
    [Header("Pacing")]
    [SerializeField]
    private float moveOffset = 2.5f;

    [SerializeField]
    private float speedTime = 20f;

    [Header("Jump")]
    [SerializeField]
    private float jumpForce = 15f;

    [SerializeField]
    private float downForce = -30f;

    [SerializeField]
    private float gravityScaleMultiplier = 3f;
    private const float Gravity = -9.81f;
    private float targetX;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            targetX -= moveOffset;
            targetX = Mathf.Clamp(targetX, -moveOffset, moveOffset);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            targetX += moveOffset;
            targetX = Mathf.Clamp(targetX, -moveOffset, moveOffset);
        }

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }

        if (Input.GetKey(KeyCode.S))
        {
            if (!IsGrounded())
            {
                rb.velocity = new Vector3(rb.velocity.x, downForce, rb.velocity.z);
            }
            else
            {
                transform.localScale = new Vector3(1, 0.8f, 1);
            }
        }
        else
        {
            transform.localScale = new Vector3(1, 2, 1);
        }
    }

    private void FixedUpdate()
    {
        Vector3 pos = Vector3.MoveTowards(
            rb.position,
            new Vector3(targetX, rb.position.y, rb.position.z),
            speedTime * Time.fixedDeltaTime
        );
        rb.MovePosition(pos);

        rb.AddForce(Gravity * gravityScaleMultiplier * Vector3.up, ForceMode.Acceleration);
    }

    private bool IsGrounded()
    {
        return Mathf.Approximately(rb.velocity.y, 0);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            GameManager.Instance.RestartGame();
        }
    }
}
