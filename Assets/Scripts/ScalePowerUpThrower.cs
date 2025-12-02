using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePowerUpThrower : MonoBehaviour
{
    [SerializeField] private TrajectoryLine trajectoryLine;
    private Vector3 originalScale;
    
    private void Awake()
    {
        originalScale = transform.localScale;
    }
    
    public void PlotTrajectory(Vector3 mousePos, Vector2 maxMag)
    {
        trajectoryLine.RenderTrajectory(transform.position, mousePos, Physics2D.gravity * Player.Instance.Rb.gravityScale, maxMag);
    }

    public void ResetTrajectory()
    {
        trajectoryLine.ResetTrajectory();
    }

    public void ThrowPowerUp(ScalePowerUp.PowerUpType powerUpType, Vector3 mousePos)
    {
        ScalePowerUp powerUp = ScalePowerUpManager.Instance.RequestScalePowerUp(powerUpType);
        if (powerUp == null) return;

        ResetTrajectory();
        Vector2 mouseClamped = new Vector2(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width), Mathf.Clamp(Input.mousePosition.y, 0, Screen.height));
        powerUp.transform.position = transform.position;
        
        powerUp.rb.AddForce(mousePos, ForceMode2D.Impulse);
        powerUp.rb.angularVelocity = 1000f;
        SoundManager.instance.Play("Shoot");
    }
}
