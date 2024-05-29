using UnityEngine;

public class PlayerControllerDecorative : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private Transform shadow;

    [Header("Configs")]
    [Header("Jump")]
    [SerializeField]
    private float jumpForce = 15f;

    [SerializeField]
    private float gravityScaleMultiplier = 3f;

    [SerializeField]
    private float gravityFallMultiplier = 2f;
    private float currentGravityScale;

    [Header("Crouch")]
    [SerializeField]
    private float scaleMultiplier = 0.4f;

    [SerializeField]
    private float downForce = 20f;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private Animator animator;
    private const float Gravity = -9.81f;
    private Vector3 originalShadowScale;
    private Vector3 originalColliderCenter;
    private Vector3 originalColliderSize;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
        rb.useGravity = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }

        animator.SetBool("IsJumping", !IsGrounded());

        if (Input.GetKey(KeyCode.S))
        {
            animator.SetBool("IsCrouching", true);
            if (!IsGrounded())
            {
                rb.AddForce(downForce * Vector3.down, ForceMode.Impulse);
            }
            else
            {
                boxCollider.center = new Vector3(
                    boxCollider.size.x,
                    originalColliderCenter.y * scaleMultiplier,
                    boxCollider.size.z
                );
                boxCollider.size = new Vector3(
                    boxCollider.size.x,
                    originalColliderSize.y * scaleMultiplier,
                    boxCollider.size.z
                );
            }
        }

        if (Input.GetKeyUp(KeyCode.S))
        {
            animator.SetBool("IsCrouching", false);
            boxCollider.center = originalColliderCenter;
            boxCollider.size = originalColliderSize;
        }

        AdjustPlayerShadow();
    }

    private void FixedUpdate()
    {
        currentGravityScale =
            rb.velocity.y < 0
                ? gravityScaleMultiplier * gravityFallMultiplier
                : gravityScaleMultiplier;
        rb.AddForce(Gravity * currentGravityScale * Vector3.up, ForceMode.Acceleration);
    }

    private bool IsGrounded() => Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);

    private void AdjustPlayerShadow()
    {
        shadow.position = new Vector3(transform.position.x, 0.01f, transform.position.z);

        float shadowScale = Mathf.Lerp(
            originalShadowScale.x,
            originalShadowScale.x * 0.8f,
            transform.position.y
        );
        shadow.localScale = new Vector3(shadowScale, originalShadowScale.y, shadowScale);
    }
}
