using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowVisual : MonoBehaviour
{
    public ScalableObject scalable;
    [SerializeField] private float arrowScaleXYZ = 0.08f;
    [SerializeField] private bool isInside;
    public enum ArrowType
    {
        Top,
        Bottom,
        Left,
        Right
    }
    [SerializeField] private ArrowType arrowType;
    private Func<bool, bool> scaleCheck;

    private void Start()
    {
        switch (arrowType)
        {
            case ArrowType.Top:
                scaleCheck = scalable.CanScaleOnYUp;
                break;
            case ArrowType.Bottom:
                scaleCheck = scalable.CanScaleOnYDown;
                break;
            case ArrowType.Left:
                scaleCheck = scalable.CanScaleOnXDown;
                break;
            case ArrowType.Right:
                scaleCheck = scalable.CanScaleOnXUp;
                break;
            default:
                break;
        }

        // arrowScale += 0.03f;
    }

    public void SetupArrow()
    {
        if (scaleCheck == null) return;
        gameObject.SetActive(scaleCheck(isInside));
    }

    public void NormalizeArrowScale()
    {
        SetupArrow();

        Vector2 localScaleAmount = transform.localRotation * scalable.transform.localScale;

        Vector2 scale = new Vector2(arrowScaleXYZ, arrowScaleXYZ);
        localScaleAmount = new Vector2(Mathf.Abs(localScaleAmount.x), Mathf.Abs(localScaleAmount.y));
        Vector3 newScale = new Vector3(scale.x / localScaleAmount.x, scale.y / localScaleAmount.y, 1f);

        // check if infinite scale
        if (float.IsInfinity(newScale.x) || float.IsInfinity(newScale.y))
        {
            newScale = Vector3.one;
        }

        transform.localScale = newScale;
    }
}
