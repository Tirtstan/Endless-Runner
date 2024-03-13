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
    private float gravityScaleMultiplier = 3f;

    [Header("Crouch")]
    [SerializeField]
    private float scaleMultiplier = 0.4f;

    [SerializeField]
    private float downForce = 20f;
    private Vector3 originalScale;
    private const float Gravity = -9.81f;
    private float targetX;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        originalScale = transform.localScale;
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
                rb.AddForce(downForce * Vector3.down, ForceMode.Impulse);
            }
            else
            {
                transform.localScale = new Vector3(
                    transform.localScale.x,
                    originalScale.y * scaleMultiplier,
                    transform.localScale.z
                );
            }
        }
        else
        {
            transform.localScale = originalScale;
        }

        if (rb.velocity.y < -20f) // for if the player falls
        {
            GameManager.Instance.RestartGame();
        }
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = Vector3.MoveTowards(
            rb.position,
            new Vector3(targetX, rb.position.y, rb.position.z),
            speedTime * Time.fixedDeltaTime
        );
        rb.MovePosition(targetPos);

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
