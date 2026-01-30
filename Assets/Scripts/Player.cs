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

    public float scaleMult = 1f;
    [Header("Coyote Time")]
    [SerializeField] private float coyoteTimeLength = 0.1f;


    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Collider2D collider;

    [SerializeField] private Animator _animator;
    public Collider2D Collider => collider;
    private PlayerInputActions input;
    private bool isGrounded;
    private float horizontalInput;
    private float velocityXSmoothing;
    private float jumpBufferCount;
    private float coyoteTimeCounter;
    private Vector3 originalScale;
    public LevelScaler currentLevelScaler;
    
    public static Player Instance;
    public Rigidbody2D Rb => rb;
    
    private void Awake()
    {
        Instance = this;

        if (rb == null) rb = GetComponent<Rigidbody2D>();
        if (_animator == null) _animator = GetComponent<Animator>();

        input = new PlayerInputActions();
        originalScale = transform.localScale;
        groundDistance = collider.bounds.extents.y + 0.05f;
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
        if (!LevelManager.Instance.isPaused) jumpBufferCount = jumpBufferLength;
    }

    private void Update()
    {
        transform.localScale = originalScale * scaleMult;
        rb.gravityScale = scaleMult;
        jumpBufferCount -= Time.deltaTime;
        if (!LevelManager.Instance.isPaused) horizontalInput = input.Player.Move.ReadValue<float>();
        float colliderxSize = collider.bounds.size.x/3;
        RaycastHit2D hit = new RaycastHit2D();
        for (int i = -1; i <=1; i++)
        {
            RaycastHit2D wallHit = Physics2D.Raycast(transform.position + new Vector3(i*colliderxSize,0,0), Vector2.down, (groundDistance + 0.01f) * scaleMult, groundMask);
            Debug.DrawRay(transform.position + new Vector3(i*colliderxSize,0,0), Vector2.down * (groundDistance + 0.01f) * scaleMult, Color.red);
            if (wallHit.collider != null)
            {
                hit = wallHit;
            }
        }
        isGrounded = hit.collider != null;
        _animator.SetFloat("Yspeed", Mathf.Abs(rb.linearVelocity.y));
        if(hit.collider != null && hit.collider.attachedRigidbody != null){
            if (hit.collider.attachedRigidbody)
            {
                hit.collider.attachedRigidbody.linearVelocity -= new Vector2(rb.linearVelocity.x, 0f) * 0.01f;
            }
        }
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
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * scaleMult);
            SoundManager.instance.Play("Jump");
            jumpBufferCount = 0;
        }
    }


    private void FixedUpdate()
    {
        float targetVelocityX = horizontalInput * speed * scaleMult;
        _animator.SetFloat("Speed", Mathf.Abs(targetVelocityX));
        float smoothSpeed = Mathf.SmoothDamp(rb.linearVelocity.x, targetVelocityX, ref velocityXSmoothing, (isGrounded ? accelerationGrounded : accelerationAirborne));
        rb.linearVelocity = new Vector2(smoothSpeed, rb.linearVelocity.y);

        //rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }
}
