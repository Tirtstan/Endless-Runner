using System;
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
    private float moveOffset = 2.5f;

    [SerializeField]
    private float speedTime = 5f;

    [SerializeField]
    private float jumpForce = 10f;

    [SerializeField]
    private float gravityScaleMultiplier = 2f;
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

        transform.position = Vector3.MoveTowards(
            transform.position,
            new Vector3(targetX, transform.position.y, transform.position.z),
            speedTime * Time.deltaTime
        );

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
        return Mathf.Approximately(rb.velocity.y, 0);
    }
}
