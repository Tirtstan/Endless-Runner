using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("Components")]
    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask groundLayer;

    [SerializeField]
    private Transform shadow;

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

    [SerializeField]
    private float gravityFallMultiplier = 2f;
    private float currentGravityScale;

    [SerializeField]
    private float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    [Header("Crouch")]
    [SerializeField]
    private float scaleMultiplier = 0.4f;

    [SerializeField]
    private float downForce = 20f;
    private Rigidbody rb;
    private BoxCollider boxCollider;
    private Animator animator;
    private const float Gravity = -9.81f;
    private float xTarget;
    private bool usingGravity = true;
    private float originalGravityScale;
    private Vector3 originalShadowScale;
    private Vector3 originalColliderCenter;
    private Vector3 originalColliderSize;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        boxCollider = GetComponent<BoxCollider>();
        animator = GetComponent<Animator>();
        rb.useGravity = false;

        originalColliderCenter = boxCollider.center;
        originalColliderSize = boxCollider.size;

        originalShadowScale = shadow.localScale;
        currentGravityScale = gravityScaleMultiplier;
        originalGravityScale = gravityFallMultiplier;
    }

    private void Update()
    {
        jumpBufferCounter -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.A))
        {
            xTarget -= moveOffset;
            xTarget = Mathf.Clamp(xTarget, -moveOffset, moveOffset);
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            xTarget += moveOffset;
            xTarget = Mathf.Clamp(xTarget, -moveOffset, moveOffset);
        }

        if (jumpBufferCounter > 0)
        {
            Jump(jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }

        animator.SetBool("IsJumping", !IsGrounded());

        if (Input.GetKey(KeyCode.S) && usingGravity) // press s and not flying
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

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.Instance.TogglePauseMenu();
        }

        AdjustPlayerShadow();
    }

    private void FixedUpdate()
    {
        rb.position = Vector3.MoveTowards(
            rb.position,
            new Vector3(xTarget, rb.position.y, rb.position.z),
            speedTime * Time.fixedDeltaTime
        );

        if (usingGravity)
        {
            currentGravityScale =
                rb.velocity.y < 0
                    ? gravityScaleMultiplier * gravityFallMultiplier
                    : gravityScaleMultiplier;
            rb.AddForce(Gravity * currentGravityScale * Vector3.up, ForceMode.Acceleration);
        }
    }

    private void Jump(float power)
    {
        if (!IsGrounded())
            return;

        rb.velocity = new Vector3(rb.velocity.x, power, rb.velocity.z);
        jumpBufferCounter = 0f;
    }

    //  ChunderSon2 (2021) demonstrates...
    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
    }

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

    public void ToggleGravity(bool value)
    {
        usingGravity = value;
    }

    public void ChangeGravity(float value)
    {
        gravityScaleMultiplier = value;
    }

    public void ResetGravityMultiplier()
    {
        gravityScaleMultiplier = originalGravityScale;
    }

    #region References
    /*

    ChunderSon2. 2021. I have a weird way to check for if the player is grounded, is there any other way I could possibly rewrite it?. [Source Code].
    Available at: https://www.reddit.com/r/Unity3D/comments/l2bzmy/i_have_a_weird_way_to_check_for_if_the_player_is/ [Accessed 02 April 2024]

    */
    #endregion
}
