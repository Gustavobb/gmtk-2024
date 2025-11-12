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

    public void RenderTrajectory(Vector3 startPos, float force, Vector2 gravity)
    {
        float timeStep = curveLength / segments;
        Vector2 mouseClamped = new Vector2(Mathf.Clamp(Input.mousePosition.x, 0, Screen.width), Mathf.Clamp(Input.mousePosition.y, 0, Screen.height));
        Vector2 velocity = (Camera.main.ScreenToWorldPoint(mouseClamped) - startPos).normalized * force;
        Vector2 position = startPos;

        for (int i = 0; i < segments; i++)
        {
            points[i] = position;
            position += velocity * timeStep + 0.5f * gravity * timeStep * timeStep;
            velocity += gravity * timeStep;
        }

        Vector2 normal = (Camera.main.ScreenToWorldPoint(mouseClamped) - startPos).normalized;

        lineMaterial.SetFloat("_lerp", Mathf.Abs(normal.x) + Mathf.Abs(normal.y));
        lineRenderer.SetPositions(points);
    }

    public void ResetTrajectory()
    {
        for (int i = 0; i < segments; i++)
        {
            points[i] = Vector3.zero;
        }

        lineRenderer.SetPositions(points);
    }
}
