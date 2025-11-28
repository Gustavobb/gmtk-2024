using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using Debug = System.Diagnostics.Debug;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Movement")] [SerializeField] private float groundDistance = 0.2f, mousePosMultiplier = 10f, analogPosMultiplier = .2f;
    [SerializeField] private Vector2 maxThrowForce = new Vector2(10f, 10f);
    [SerializeField] private LayerMask groundMask;
    public GameObject groundedOn;
    public static PlayerInteraction Instance;

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ScalePowerUpThrower scalePowerUpThrower;
    public static bool isScaling = false;

    private ScalePowerUp.PowerUpType currentPowerUpType = ScalePowerUp.PowerUpType.Null;
    private CustomStopwatch throwCooldown = new CustomStopwatch();
    private Vector3 mousePosition, deltaAnalog;
    private PlayerInputActions input;
    public PlayerInput pInput;
    private bool isKeyboardAndMouse;

    private void Awake()
    {
        Instance = this;
        throwCooldown.Restart();
        SetControls(pInput);
    }

    private void OnDisable()
    {
        input.Player.Disable();
        input.UI.Disable();
        input.Disable();
    }

    public void SetControls(PlayerInput obj)
    {
        input = new PlayerInputActions();
        input.Enable();
        input.Player.ShootPlus.performed += ctx => StartThrowAction(ScalePowerUp.PowerUpType.ScaleUp);
        input.Player.ShootMinus.performed += ctx => StartThrowAction(ScalePowerUp.PowerUpType.ScaleDown);
        input.Player.CancelShot.canceled += ctx => StartThrowAction(ScalePowerUp.PowerUpType.Null);
        input.Player.ShootPlus.canceled += ctx => ThrowAction(ScalePowerUp.PowerUpType.ScaleUp);
        input.Player.ShootMinus.canceled += ctx => ThrowAction(ScalePowerUp.PowerUpType.ScaleDown);

    }
    
    private void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundDistance, groundMask);
        groundedOn = hit.collider?.gameObject;
        isKeyboardAndMouse = pInput.currentControlScheme == "keyboard&mouse";
        deltaAnalog = isKeyboardAndMouse ? Input.mousePositionDelta*mousePosMultiplier : input.Player.Aim.ReadValue<Vector2>()*analogPosMultiplier;
        if (throwCooldown.ElapsedTimeSec() < 0.2f)
        {
            return;
        }
        
        UpdateThrowAction(currentPowerUpType);
    }

    private void StartThrowAction(ScalePowerUp.PowerUpType powerUpType)
    {
        if(currentPowerUpType != ScalePowerUp.PowerUpType.Null){
            scalePowerUpThrower.ResetTrajectory();
            throwCooldown.Restart();
            currentPowerUpType = ScalePowerUp.PowerUpType.Null;
            return;
        }
        if(powerUpType == ScalePowerUp.PowerUpType.Null) return;
        if (!BulletCounter.Instance.HasBullet(powerUpType)) return;
        currentPowerUpType = powerUpType;
        float dot = Vector3.Dot(-transform.right, (transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition)).normalized);
        float distance = Vector3.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        // mousePosition = dot < 0 ? new Vector3(-1, 1, 0) * distance : new Vector3(1, 1, 0) * distance;
    }

    private void UpdateThrowAction(ScalePowerUp.PowerUpType powerUpType)
    {
        if (powerUpType == ScalePowerUp.PowerUpType.Null || powerUpType == null) return;
        if (!BulletCounter.Instance.HasBullet(powerUpType)) return;
        mousePosition += deltaAnalog * Time.deltaTime;
        mousePosition = new Vector3(
            Mathf.Clamp(mousePosition.x, -maxThrowForce.x, maxThrowForce.x),
            Mathf.Clamp(mousePosition.y, -maxThrowForce.y, maxThrowForce.y),
            mousePosition.z);
        scalePowerUpThrower.PlotTrajectory(mousePosition, maxThrowForce);
    }
    
    private void ThrowAction(ScalePowerUp.PowerUpType powerUpType)
    {
        if (currentPowerUpType == ScalePowerUp.PowerUpType.Null || currentPowerUpType == null) return;
        scalePowerUpThrower.ResetTrajectory();
        if (!BulletCounter.Instance.HasBullet(powerUpType)) return;
        scalePowerUpThrower.ThrowPowerUp(powerUpType, mousePosition);
        BulletCounter.Instance.UseBullet(powerUpType);
        throwCooldown.Restart();
        currentPowerUpType = ScalePowerUp.PowerUpType.Null;
    }

    public void Propel(Vector2 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}
