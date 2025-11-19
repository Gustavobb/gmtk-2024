using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


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

    private PlayerInputActions input;
    private bool isGrounded;
    private float horizontalInput;
    private float velocityXSmoothing;
    private float jumpBufferCount;
    private float coyoteTimeCounter;
    
    public static Player Instance;
    
    private void Awake()
    {
        Instance = this;

        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (_animator == null) _animator = GetComponent<Animator>();

        input = new PlayerInputActions();
    }

    private void OnEnable()
    {
        input.Enable();

        input.Player.Jump.performed += OnJumpPerformed;
    }

    private void OnDisable()
    {
        input.Player.Jump.performed -= OnJumpPerformed;
        input.Disable();
    }

    private void OnJumpPerformed(InputAction.CallbackContext ctx)
    {
        jumpBufferCount = jumpBufferLength;
    }

    private void Update()
    {
        jumpBufferCount -= Time.deltaTime;
        horizontalInput = input.Player.Move.ReadValue<float>();
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask);

        isGrounded = hit.collider != null;
        _animator.SetFloat("Yspeed", Mathf.Abs(rb.linearVelocity.y));

        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTimeLength;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        if (jumpBufferCount >= 0 && coyoteTimeCounter > 0 && rb.linearVelocity.y <= 0)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            SoundManager.instance.Play("Jump");
            jumpBufferCount = 0;
        }
    }


    private void FixedUpdate()
    {
        float targetVelocityX = horizontalInput * speed;

        _animator.SetInteger("Speed", Mathf.Abs((int)targetVelocityX));

        float smoothSpeed = Mathf.SmoothDamp(rb.linearVelocity.x, targetVelocityX, ref velocityXSmoothing, isGrounded ? accelerationGrounded : accelerationAirborne);

        rb.linearVelocity = new Vector2(smoothSpeed, rb.linearVelocity.y);

        //rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }
}
