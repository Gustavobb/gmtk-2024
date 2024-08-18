using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePowerUp : MonoBehaviour
{
    public enum PowerUpType
    {
        ScaleUp,
        ScaleDown
    }

    [SerializeField] private PowerUpType powerUpType;
    public Rigidbody2D rb;
    public ScalePowerUpManager scalePowerUpManager;

    private const string SCALABLE_TAG = "Scalable";
    private const string CONNECTOR_TAG = "Connector";
    private const string POWER_UP_BOUNCER_TAG = "PowerUpBouncer";

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(POWER_UP_BOUNCER_TAG))
        {
            rb.AddForce(collision.GetContact(0).normal * 10f, ForceMode2D.Impulse);
            return;
        }

        if (collision.gameObject.CompareTag(SCALABLE_TAG) || collision.gameObject.CompareTag(CONNECTOR_TAG)) 
        {
            ScalableObject scalable = collision.gameObject.GetComponent<ScalableObject>();
            if (scalable == null) return;

            RunFunctionDependingOnPowerUpType(
                () => scalable.ScaleUpQueue(),
                () => scalable.ScaleDownQueue()
            );
        }

        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        RunFunctionDependingOnPowerUpType(
            () => scalePowerUpManager.ReturnScaleUpPowerUp(gameObject),
            () => scalePowerUpManager.ReturnScaleDownPowerUp(gameObject)
        );
    }

    private void RunFunctionDependingOnPowerUpType(Action funcScaleUp, Action funcScaleDown)
    {
        switch (powerUpType)
        {
            case PowerUpType.ScaleUp:
                funcScaleUp();
                break;
            case PowerUpType.ScaleDown:
                funcScaleDown();
                break;
        }
    }
}
