using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePowerUpThrower : MonoBehaviour
{
    [SerializeField] private TrajectoryLine trajectoryLine;

    [Header("Throwing")]
    public float throwForce = 10f;

    public void PlotTrajectory()
    {
        trajectoryLine.RenderTrajectory(transform.position, throwForce, Physics2D.gravity);
    }

    public void ResetTrajectory()
    {
        trajectoryLine.ResetTrajectory();
    }

    public void ThrowPowerUp(ScalePowerUp.PowerUpType powerUpType)
    {
        ScalePowerUp powerUp = ScalePowerUpManager.Instance.RequestScalePowerUp(powerUpType);
        if (powerUp == null) return;

        ResetTrajectory();
        Vector2 mouseClamped = new Vector2(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width), Mathf.Clamp(Input.mousePosition.y, 0, Screen.height));

        powerUp.transform.position = transform.position;
        Vector2 direction = (Camera.main.ScreenToWorldPoint(mouseClamped) - transform.position).normalized;
        powerUp.rb.AddForce(direction * throwForce, ForceMode2D.Impulse);
        powerUp.rb.angularVelocity = 1000f;
        SoundManager.instance.Play("Shoot");
    }
}
