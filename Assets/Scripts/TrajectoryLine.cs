using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [Header("Trajectory")]
    [SerializeField] private int segments = 50;
    [SerializeField] private float curveLength = 3.5f;
    [SerializeField] private Material lineMaterial;

    [Header("References")]
    [SerializeField] private LineRenderer lineRenderer;
    private Vector3[] points;

    private void Start()
    {
        points = new Vector3[segments];
        lineRenderer.positionCount = segments;
    }

    public void RenderTrajectory(Vector3 startPos, Vector3 mousePos, Vector2 gravity, Vector2 maxMag)
    {
        float timeStep = curveLength / segments;
        Vector2 velocity = mousePos;
        Vector2 position = startPos;

        for (int i = 0; i < segments; i++)
        {
            points[i] = position;
            position += velocity * timeStep + 0.5f * gravity * timeStep * timeStep;
            velocity += gravity * timeStep;
        }

        float maxVal = Mathf.Max(Mathf.Abs(mousePos.x) / maxMag.x, Mathf.Abs(mousePos.y) / maxMag.y);
        lineMaterial.SetFloat("_lerp", maxVal);
        lineRenderer.SetPositions(points);
        lineRenderer.textureScale = new Vector2(1f / Player.Instance.scaleMult, 1f);
    }

    public void ResetTrajectory()
    {
        StartCoroutine(Util.AnimateFloat((float v) =>
        {
            lineRenderer.widthMultiplier = v;
            return 0;
        }, 1f, 0f, 0.2f, () =>
        {
            for (int i = 0; i < segments; i++)
            {
                points[i] = Vector3.zero;
            }
        
            lineRenderer.SetPositions(points);
            lineRenderer.widthMultiplier = 1f;
        }));
    }
}
