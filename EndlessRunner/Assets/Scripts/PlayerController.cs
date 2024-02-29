using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask groundLayer;

    [Header("Configs")]
    [SerializeField]
    private float speed = 15f;

    [SerializeField]
    private float speedTime = 50f;

    [SerializeField]
    private float jumpForce = 10f;

    [SerializeField]
    private float gravityScaleMultiplier = 2f;
    private const float Gravity = -9.81f;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");

        float xMove = Mathf.MoveTowards(
            rb.velocity.x,
            speed * horizontalInput,
            speedTime * Time.deltaTime
        );
        rb.velocity = new Vector3(xMove, rb.velocity.y, rb.velocity.z);

        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    private void FixedUpdate()
    {
        rb.AddForce(Gravity * gravityScaleMultiplier * Vector3.up, ForceMode.Acceleration);
    }

    private bool IsGrounded()
    {
        return rb.velocity.y.CompareTo(0) == 0;
    }
}
