using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScaler : MonoBehaviour
{
    [SerializeField] private Player player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Collider2D triggerCollider;
    [SerializeField] private float scaleFactor = 1.2f;
    [SerializeField] private float effectTime = 1f;
    [SerializeField] private LevelScaler parentScaler;
    private IEnumerator coroutine;
    private Vector3 startCameraPos;
    private float startCameraSize, endCameraSize;
    private float startPlayerScaleMult, endPlayerScaleMult;
    private bool isInside = false, wasInside = false;

    private void Start()
    {
        float parentScaleDepth = 1f;
        
        startCameraPos = mainCamera.transform.position;
        if (parentScaler != null)
        {
            startCameraPos = parentScaler.transform.position;
            parentScaleDepth = parentScaler.RecursiveGetScaleDepth();
        }

        startCameraSize = mainCamera.orthographicSize * parentScaleDepth;
        endCameraSize = startCameraSize * scaleFactor;
        
        startPlayerScaleMult = player.scaleMult * parentScaleDepth;
        endPlayerScaleMult = startPlayerScaleMult * scaleFactor;
        
        if (triggerCollider == null)
        {
            triggerCollider = GetComponent<Collider2D>();
        }
    }

    private float RecursiveGetScaleDepth()
    {
        return scaleFactor * (parentScaler != null ? parentScaler.RecursiveGetScaleDepth() : 1f);
    }

    private bool PlayerIsInsideCollider()
    {
        return triggerCollider.OverlapPoint(player.transform.position);
    }

    private void Update()
    {
        wasInside = isInside;
        isInside = PlayerIsInsideCollider();
        
        if (isInside == wasInside) return;
        
        if (isInside && coroutine == null)
        {
            OnPlayerEnter2D();
            return;
        }
        
        if (!isInside && coroutine == null)
        {
            OnPlayerExit2D();
        }
    }
    
    private void OnPlayerEnter2D()
    {
        coroutine = ScaleEffect(true);
        StartCoroutine(coroutine);
    }

    private void OnPlayerExit2D()
    {
        coroutine = ScaleEffect(false);
        StartCoroutine(coroutine);
    }
    
    private IEnumerator ScaleEffect(bool zoomIn)
    {
        float startSize = mainCamera.orthographicSize;
        float startScaleMult = player.scaleMult;
        Vector3 startPos = mainCamera.transform.position;
        startPos.z = -10f;
        
        float targetSize = zoomIn ? endCameraSize : startCameraSize;
        float targetPlayerScaleMult = zoomIn ? endPlayerScaleMult : startPlayerScaleMult;
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