using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float speed = 5f;
    [SerializeField] private float jumpForce = 10f;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;

    [Header("Jump Buffer")]
    [SerializeField] private float jumpBufferLength = 0.1f;
	private float jumBufferCount;

    [Header("Coyote Time")]
    [SerializeField] private float coyoteTimeLength = 0.1f;
    private float coyoteTimeCounter;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    private bool isGrounded;
    private float horizontalInput;

    private void Update()
    {
        jumBufferCount -= Time.deltaTime;
        horizontalInput = Input.GetAxis("Horizontal");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask);

        isGrounded = hit.collider != null;

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTimeLength;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumBufferCount = jumpBufferLength;
            //rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (jumBufferCount >= 0 && coyoteTimeCounter > 0)
		{
			rb.velocity = new Vector2(rb.velocity.x, jumpForce);
			jumBufferCount = 0;
		}
    }
    

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }
}
