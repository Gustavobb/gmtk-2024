using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsideBox : MonoBehaviour
{
    public bool isPlayerInside;
    private const string PLAYER_TAG = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(PLAYER_TAG)) return;
        isPlayerInside = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(PLAYER_TAG)) return;
        isPlayerInside = false;
    }
}
