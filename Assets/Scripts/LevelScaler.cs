using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScaler : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float scaleFactor = 1.2f;
    [SerializeField] private float effectTime = 1f;
    private const string PLAYER_TAG = "Player";
    private IEnumerator coroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(PLAYER_TAG)) return;

        if (coroutine != null)
        {
            return;
        }
        
        startCameraSize = mainCamera.orthographicSize;
        startCameraPos = mainCamera.transform.position;
        startPlayerScaleMult = player.scaleMult;
        coroutine = ScaleEffect(true);
        StartCoroutine(coroutine);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag(PLAYER_TAG)) return;
        if (coroutine != null) return;
        
        coroutine = ScaleEffect(false);
        StartCoroutine(coroutine);
    }
    
    private Vector3 startCameraPos;
    private float startCameraSize, startPlayerScaleMult;
    
    private IEnumerator ScaleEffect(bool zoomIn)
    {
        float startSize = mainCamera.orthographicSize;
        float startScaleMult = player.scaleMult;
        Vector3 startPos = mainCamera.transform.position;
        startPos.z = -10f;
        
        float targetSize = zoomIn ? startCameraSize * scaleFactor : startCameraSize;
        float targetPlayerScaleMult = zoomIn ? startPlayerScaleMult * scaleFactor : startPlayerScaleMult;
        Vector3 targetPos = zoomIn ? transform.position : startCameraPos;
        targetPos.z = -10f;
        
        float elapsed = 0f;
        
        while (elapsed < effectTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / effectTime);
            t = zoomIn ? Easing.EaseOutSinusoidal(elapsed / effectTime)
                : Easing.EaseInSinusoidal(elapsed / effectTime);
            player.scaleMult = Mathf.Lerp(startScaleMult, targetPlayerScaleMult, t);
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, t);
            mainCamera.transform.position = Vector3.Lerp(startPos, targetPos, t);
            yield return null;
        }

        mainCamera.orthographicSize = targetSize;
        mainCamera.transform.position = targetPos;
        player.scaleMult = targetPlayerScaleMult;
        coroutine = null;
    }
}
