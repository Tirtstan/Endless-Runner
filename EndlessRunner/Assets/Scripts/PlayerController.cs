using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public static event Action OnPlayerDeath;

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
    private Vector3 originalScale;
    private const float Gravity = -9.81f;
    private float targetX;
    private Rigidbody rb;
    private bool usingGravity = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;

        originalScale = transform.localScale;
        currentGravityScale = gravityScaleMultiplier;
    }

    private void Update()
    {
        jumpBufferCounter -= Time.deltaTime;

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

        if (jumpBufferCounter > 0)
        {
            Jump(jumpForce);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferCounter = jumpBufferTime;
        }

        if (Input.GetKey(KeyCode.S) && usingGravity) // press s and not flying
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

        if (transform.position.y < -20f) // for if the player falls off map
        {
            GameManager.Instance.RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseMenu.Instance.TogglePauseMenu();
        }

        AdjustPlayerShadow();
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = Vector3.MoveTowards(
            rb.position,
            new Vector3(targetX, rb.position.y, rb.position.z),
            speedTime * Time.fixedDeltaTime
        );
        rb.MovePosition(targetPos);

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

    public void ToggleGravity(bool value)
    {
        usingGravity = value;
    }

    //  ChunderSon2 (2021) demonstrates...
    private bool IsGrounded()
    {
        return Physics.CheckSphere(groundCheck.position, 0.2f, groundLayer);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            OnPlayerDeath?.Invoke();
        }
    }

    private void AdjustPlayerShadow()
    {
        shadow.position = new Vector3(transform.position.x, 0.01f, transform.position.z);

        float shadowScale = Mathf.Lerp(0.125f, 0.05f, transform.position.y / 5f);
        shadow.localScale = Vector3.one * shadowScale;
    }

    #region References
    /*

    ChunderSon2. 2021. I have a weird way to check for if the player is grounded, is there any other way I could possibly rewrite it?. [Source Code].
    Available at: https://www.reddit.com/r/Unity3D/comments/l2bzmy/i_have_a_weird_way_to_check_for_if_the_player_is/ [Accessed 02 April 2024]

    */
    #endregion
}
