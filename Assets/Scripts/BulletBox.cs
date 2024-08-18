using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBox : MonoBehaviour
{
    [SerializeField] private BulletCounter bulletCounter;
    [SerializeField] private int plusGain, minusGain;
    private float xr, yr, zr;

    private void Start()
    {
        GameObjectUpdateManager.PerformUpdate += PerformUpdate;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        bulletCounter.AddPlusBullets(plusGain);
        bulletCounter.AddMinusBullets(minusGain);
        gameObject.SetActive(false);
    }

    private void PerformUpdate()
    {
        if (!gameObject.activeSelf) return;
        
        xr += Time.deltaTime * 50f;
        yr -= Time.deltaTime * 10f;
        zr += Time.deltaTime * 30f;

        transform.rotation = Quaternion.Euler(xr, yr, zr);
    }
}
