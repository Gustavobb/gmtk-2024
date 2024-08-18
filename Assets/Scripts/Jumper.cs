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
        if (Player.Instance.groundedOn != gameObject) return;
        Player.Instance.Propel(Vector2.up, propelForce);
    }
}
