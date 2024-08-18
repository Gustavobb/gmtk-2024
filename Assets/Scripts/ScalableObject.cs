using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ScalableObject : MonoBehaviour
{
    public enum ScaleBehaviour
    {
        AxisPlus,
        AxisMinus,
        Uniform,
        None
    }
    [SerializeField] private ScaleBehaviour scaleBehaviourX = ScaleBehaviour.Uniform;
    [SerializeField] private ScaleBehaviour scaleBehaviourY = ScaleBehaviour.Uniform;

    [Header("Values")]
    [SerializeField] private Vector3 minScale = new Vector3(0.5f, 0.5f, 0.5f);
    [SerializeField] private Vector3 maxScale = new Vector3(1.5f, 1.5f, 1.5f);
    [SerializeField] private Vector3 scaleAmount = new Vector3(0.5f, 0.5f, 0.5f);

    [Header("Easing")]
    [SerializeField] private float duration = 1f;
    [SerializeField] private Easing.EasingType easingType = Easing.EasingType.Linear;
    private Func<float, float> ease;
    private Queue<Action> scaleFunctions = new Queue<Action>();
    public bool isScaling;
    public UnityEvent onScaleDown, onScaleUp;

#if UNITY_EDITOR
    private Easing.EasingType lastEasingType;
    
    private void Update()
    {
        if (lastEasingType != easingType)
        {
            ease = Easing.GetEasingFunction(easingType);
            lastEasingType = easingType;
        }
    }
#endif

    void OnDestroy(){
        GameObjectUpdateManager.PerformUpdate -= PerformUpdate;
    }

    public void PerformUpdate()
    {
        // tava dando erro no reset
        if (!gameObject.activeInHierarchy) return;

        if (scaleFunctions.Count > 0 && !isScaling)
        {
            isScaling = true;
            scaleFunctions.Dequeue().Invoke();
        }
    }

    private void Start()
    {
        ease = Easing.GetEasingFunction(easingType);
        GameObjectUpdateManager.PerformUpdate += PerformUpdate;
    }

    public void ScaleDownQueue()
    {
        scaleFunctions.Enqueue(ScaleDown);
    }

    public void ScaleUpQueue()
    {
        scaleFunctions.Enqueue(ScaleUp);
    }

    [EButton]
    public void ScaleDown()
    {
        Vector3 scale = -scaleAmount;
        Vector3 translation = Vector3.zero;
        
        if ((transform.localScale + scale).x < minScale.x) scale.x = minScale.x - transform.localScale.x;
        if ((transform.localScale + scale).y < minScale.y) scale.y = minScale.y - transform.localScale.y;
        CalculateScaleBehaviour(ref scale, ref translation);

        onScaleDown?.Invoke();
        StartCoroutine(ScaleCoroutine(scale, translation));
    }

    [EButton]
    public void ScaleUp()
    {
        Vector3 scale = scaleAmount;
        Vector3 translation = Vector3.zero;

        if ((maxScale - transform.localScale).x < scale.x) scale.x = maxScale.x - transform.localScale.x;
        if ((maxScale - transform.localScale).y < scale.y) scale.y = maxScale.y - transform.localScale.y;
        CalculateScaleBehaviour(ref scale, ref translation);
        
        onScaleUp?.Invoke();
        StartCoroutine(ScaleCoroutine(scale, translation));
    }

    private void CalculateScaleBehaviour(ref Vector3 scale, ref Vector3 translation)
    {
        switch (scaleBehaviourX)
        {
            case ScaleBehaviour.AxisPlus:
                translation.x = scale.x / 2f;
                break;
            case ScaleBehaviour.AxisMinus:
                translation.x = -scale.x / 2f;
                break;
            case ScaleBehaviour.Uniform:
                translation.x = 0f;
                break;
            case ScaleBehaviour.None:
                translation.x = 0f;
                scale.x = 0f;
                break;
            default:
                break;
        }

        switch (scaleBehaviourY)
        {
            case ScaleBehaviour.AxisPlus:
                translation.y = scale.y / 2f;
                break;
            case ScaleBehaviour.AxisMinus:
                translation.y = -scale.y / 2f;
                break;
            case ScaleBehaviour.Uniform:
                translation.y = 0f;
                break;
            case ScaleBehaviour.None:
                translation.y = 0f;
                scale.y = 0f;
                break;
            default:
                break;
        }
    }

    private IEnumerator ScaleCoroutine(Vector3 scaleAmount, Vector3 translationAmount)
    {
        float time = 0f;
        Vector3 initialScale = transform.localScale;
        Vector3 endScale = initialScale + scaleAmount;

        Vector3 initialPosition = transform.localPosition;
        Vector3 endPosition = initialPosition + translationAmount;

        while (time < duration)
        {
            transform.localScale = Vector3.Lerp(initialScale, endScale, ease(time / duration));
            transform.localPosition = Vector3.Lerp(initialPosition, endPosition, ease(time / duration));

            time += Time.deltaTime;
            yield return null;
        }

        transform.localScale = endScale;
        transform.localPosition = endPosition;
        isScaling = false;
    }
}
