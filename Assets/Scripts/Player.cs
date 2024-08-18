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
    public GameObject groundedOn;
    public static Player Instance;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ScalePowerUpThrower scalePowerUpThrower;
    private bool isGrounded;
    private float horizontalInput;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Update()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask);

        isGrounded = hit.collider != null;
        groundedOn = hit.collider?.gameObject;
        
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }

        if (Input.GetMouseButtonUp(0))
        {
            scalePowerUpThrower.ThrowPowerUp(ScalePowerUp.PowerUpType.ScaleUp);
            return;
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            scalePowerUpThrower.ThrowPowerUp(ScalePowerUp.PowerUpType.ScaleDown);
            return;
        }

        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            // plot()
            scalePowerUpThrower.PlotTrajectory();
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalInput * speed, rb.velocity.y);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = isGrounded ? Color.green : Color.red;
        Gizmos.DrawLine(transform.position, transform.position + Vector3.down * groundDistance);
    }

    public void Propel(Vector2 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
