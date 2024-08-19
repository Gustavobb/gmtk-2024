using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jumper : MonoBehaviour
{
    [SerializeField] private ScalableObject scalable;
    [SerializeField] private float propelForce = 5f;
    private const string PLAYER_TAG = "Player";

    private void Start()
    {
        scalable.onScaleUp.AddListener(CheckIfCanPropelPlayer);
    }

    private void CheckIfCanPropelPlayer()
    {
        if (PlayerInteraction.Instance.groundedOn != gameObject) return;
        PlayerInteraction.Instance.Propel(scalable.ScaleDirection, propelForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag(PLAYER_TAG) || PlayerInteraction.Instance.groundedOn == gameObject) return;
        if (!scalable.isScaling) return;
        PlayerInteraction.Instance.Propel(scalable.ScaleDirection, propelForce);
    }
}
