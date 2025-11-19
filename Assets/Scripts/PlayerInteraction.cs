using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Movement")] [SerializeField] private float groundDistance = 0.2f, mousePosMultiplier = 5f;
    [SerializeField] private Vector2 maxThrowForce = new Vector2(10f, 10f);
    [SerializeField] private LayerMask groundMask;
    public GameObject groundedOn;
    public static PlayerInteraction Instance;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ScalePowerUpThrower scalePowerUpThrower;
    public static bool isScaling = false;

    private Vector3 mousePosition;
    private CustomStopwatch throwCooldown = new CustomStopwatch();

    private void Awake()
    {
        Instance = this;
        throwCooldown.Restart();
    }
    
    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask);
        groundedOn = hit.collider?.gameObject;

        if (throwCooldown.ElapsedTimeSec() < 0.2f)
        {
            return;
        }
        
        if (Input.GetMouseButtonUp(0))
        {
            scalePowerUpThrower.ResetTrajectory();
            if (BulletCounter.Instance.HasPlusBullet())
            {
                scalePowerUpThrower.ThrowPowerUp(ScalePowerUp.PowerUpType.ScaleUp, mousePosition);
                BulletCounter.Instance.UsePlusBullet();
                throwCooldown.Restart();
                return;
            }

            return;
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            scalePowerUpThrower.ResetTrajectory();
            if (BulletCounter.Instance.HasMinusBullet())
            {
                scalePowerUpThrower.ThrowPowerUp(ScalePowerUp.PowerUpType.ScaleDown, mousePosition);
                BulletCounter.Instance.UseMinusBullet();
                throwCooldown.Restart();
                return;
            }

            return;
        }
        
        if ((Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)) && mousePosition.magnitude < 0.1f)
        {
            float dot = Vector3.Dot(-transform.right, (transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized);
            mousePosition = dot < 0 ? new Vector3(-1, 2, 0) * 2f : new Vector3(1, 2, 0) * 2f;
        }

        if ((Input.GetMouseButton(0) && BulletCounter.Instance.HasPlusBullet())
            || Input.GetMouseButton(1) && BulletCounter.Instance.HasMinusBullet())
        {
            // plot()
            mousePosition += Input.mousePositionDelta * (Time.deltaTime * mousePosMultiplier);
            mousePosition = new Vector3(
                Mathf.Clamp(mousePosition.x, -maxThrowForce.x, maxThrowForce.x),
                Mathf.Clamp(mousePosition.y, -maxThrowForce.y, maxThrowForce.y),
                mousePosition.z);
            scalePowerUpThrower.PlotTrajectory(mousePosition, maxThrowForce);
        }
    }

    public void Propel(Vector2 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
