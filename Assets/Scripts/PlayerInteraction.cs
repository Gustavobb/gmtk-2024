using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
[Header("Movement")]
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private LayerMask groundMask;
    public GameObject groundedOn;
    public static PlayerInteraction Instance;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ScalePowerUpThrower scalePowerUpThrower;
    [SerializeField] private BulletCounter bulletCounter;
    private bool isGrounded;

    private void Awake()
    {
        Instance = this;
    }
    
    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask);

        isGrounded = hit.collider != null;
        groundedOn = hit.collider?.gameObject;
        
        if (Input.GetMouseButtonUp(0))
        {
            scalePowerUpThrower.ResetTrajectory();
            if (bulletCounter.HasPlusBullet())
            {
                scalePowerUpThrower.ThrowPowerUp(ScalePowerUp.PowerUpType.ScaleUp);
                bulletCounter.UsePlusBullet();
                return;
            }

            // Implementar feedback visual/sonoro
            return;
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            scalePowerUpThrower.ResetTrajectory();
            if (bulletCounter.HasMinusBullet())
            {
                scalePowerUpThrower.ThrowPowerUp(ScalePowerUp.PowerUpType.ScaleDown);
                bulletCounter.UseMinusBullet();
                return;
            }

            // Implementar feedback visual/sonoro
            return;
        }

        if (Input.GetMouseButton(0) && bulletCounter.HasPlusBullet())
        {
            // plot()
            scalePowerUpThrower.PlotTrajectory();
        }

        if (Input.GetMouseButton(1) && bulletCounter.HasMinusBullet())
        {
            // plot()
            scalePowerUpThrower.PlotTrajectory();
        }
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
