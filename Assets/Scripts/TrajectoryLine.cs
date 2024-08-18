using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryLine : MonoBehaviour
{
    [Header("Trajectory")]
    [SerializeField] private int segments = 50;
    [SerializeField] private float curveLength = 3.5f;

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
        Vector2 velocity = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - startPos).normalized * force;
        Vector2 position = startPos;

        for (int i = 0; i < segments; i++)
        {
            points[i] = position;
            position += velocity * timeStep + 0.5f * gravity * timeStep * timeStep;
            velocity += gravity * timeStep;
        }

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
