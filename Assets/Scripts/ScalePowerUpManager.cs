using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScalePowerUpManager : MonoBehaviour
{
    [SerializeField] private ScalePowerUp scaleUp, scaleDown;
    [SerializeField] private int poolSize = 5;

    private Queue<ScalePowerUp> scalePowerUpScaleUpPool = new Queue<ScalePowerUp>();
    private Queue<ScalePowerUp> scalePowerUpScaleDownPool = new Queue<ScalePowerUp>();

    private void Start()
    {
        CreatePool(scalePowerUpScaleUpPool, scaleUp);
        CreatePool(scalePowerUpScaleDownPool, scaleDown);
    }

    private void CreatePool(Queue<ScalePowerUp> pool, ScalePowerUp prefab)
    {
        for (int i = 0; i < poolSize; i++)
        {
            ScalePowerUp powerUp = Instantiate(prefab, transform);
            powerUp.scalePowerUpManager = this;
            powerUp.gameObject.SetActive(false);
        }
    }

    public void ReturnScaleUpPowerUp(GameObject powerUp)
    {
        scalePowerUpScaleUpPool.Enqueue(powerUp.GetComponent<ScalePowerUp>());
    }

    public void ReturnScaleDownPowerUp(GameObject powerUp)
    {
        scalePowerUpScaleDownPool.Enqueue(powerUp.GetComponent<ScalePowerUp>());
    }

    public ScalePowerUp RequestScalePowerUp(ScalePowerUp.PowerUpType powerUpType)
    {
        Queue<ScalePowerUp> pool = powerUpType == ScalePowerUp.PowerUpType.ScaleUp ? scalePowerUpScaleUpPool : scalePowerUpScaleDownPool;

        if (pool.Count == 0)
        {
            Debug.LogWarning("No power ups available in the pool.");
            return null;
        }

        ScalePowerUp powerUp = pool.Dequeue();
        powerUp.gameObject.SetActive(true);
        return powerUp;
    }
}
