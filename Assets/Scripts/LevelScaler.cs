using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScaler : MonoBehaviour
{
    [SerializeField] private LevelScaler parentScaler;
    [SerializeField] private LevelScaler rootScaler;
    [SerializeField] private InsideBox insideBox;
    [SerializeField] private BoxCollider2D levelBounds;
    [SerializeField] private Player player;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Vector3 finalScale = new Vector3(.5f, .5f, .5f);
    [SerializeField] private float effectTime = 1f;
    [SerializeField] private float effectDirection = 1f;
    private float effectTimeCounter;
    private Vector3 initialScale;
    private Vector3 initialCameraPosition;
    private bool performUpdate = false;
    private const string PLAYER_TAG = "Player";

    void OnDestroy(){
        GameObjectUpdateManager.PerformFixedUpdate -= PerformFixedUpdate;
    }

    private void Start()
    {
        GameObjectUpdateManager.PerformFixedUpdate += PerformFixedUpdate;
        rootScaler = GetLevelScalerParentRecursive();
        performUpdate = false;
    }

    public LevelScaler GetLevelScalerParentRecursive()
    {
        if (parentScaler == null) return this;
        return parentScaler.GetLevelScalerParentRecursive();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag(PLAYER_TAG) || performUpdate) return;
        performUpdate = true;
        initialScale = rootScaler.transform.localScale;
        initialCameraPosition = mainCamera.transform.position;
        // effectTimeCounter = 0f;
        effectDirection = 1f;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (insideBox.isPlayerInside) return;

        if (!collision.CompareTag(PLAYER_TAG)) return;
        performUpdate = true;
        effectDirection = -1f;
    }

    private void PerformFixedUpdate()
    {
        if (!gameObject.activeInHierarchy || !performUpdate) return;

        float t = effectTimeCounter / effectTime;
        t = Mathf.Clamp01(t);
        t = effectDirection > 0f ? Easing.EaseOutSinusoidal(t) : Easing.EaseOutSinusoidal(t);
        rootScaler.transform.localScale = Vector3.Lerp(initialScale, finalScale, t);
        mainCamera.transform.position = Vector3.Lerp(initialCameraPosition, transform.position, t);
        mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, -10f);
        effectTimeCounter += Time.deltaTime * effectDirection;

        bool isEffectFinished = effectDirection > 0f ? effectTimeCounter - effectTime >= 0f : effectTimeCounter <= 0f;
        if (isEffectFinished)
        {
            performUpdate = false;
            rootScaler.transform.localScale = effectDirection > 0f ? finalScale : initialScale;
            mainCamera.transform.position = effectDirection > 0f ? transform.position : initialCameraPosition;
            mainCamera.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, -10f);
            effectTimeCounter = Mathf.Clamp(effectTimeCounter, 0f, effectTime);
        }
    }
}
