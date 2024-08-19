using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float accelerationGrounded = 0.1f;
    [SerializeField] private float accelerationAirborne = 0.2f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    [Header("Jump Buffer")]
    [SerializeField] private float jumpBufferLength = 0.1f;


    [Header("Coyote Time")]
    [SerializeField] private float coyoteTimeLength = 0.1f;


    [Header("References")]
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private Animator _animator;
    private bool isGrounded;
    private float horizontalInput;
    private float velocityXSmoothing;
    private float jumBufferCount;
    private float coyoteTimeCounter;

    private void Update()
    {
        jumBufferCount -= Time.deltaTime;
        horizontalInput = Input.GetAxis("Horizontal");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask);

        isGrounded = hit.collider != null;
        _animator.SetFloat("Yspeed", Mathf.Abs(rb.velocity.y));

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTimeLength;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumBufferCount = jumpBufferLength;
            
            //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (jumBufferCount >= 0 && coyoteTimeCounter > 0 && rb.velocity.y <= 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumBufferCount = 0;
        }
    }


    private void FixedUpdate()
    {
        float targetVelocityX = horizontalInput * speed;

        _animator.SetInteger("Speed", Mathf.Abs((int)targetVelocityX));

        float smoothSpeed = Mathf.SmoothDamp(rb.velocity.x, targetVelocityX, ref velocityXSmoothing, isGrounded ? accelerationGrounded : accelerationAirborne);

        rb.velocity = new Vector2(smoothSpeed, rb.velocity.y);

        //rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }
}
