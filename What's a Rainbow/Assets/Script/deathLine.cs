using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(EdgeCollider2D))]
public class deathLine : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;

    void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        edgeCollider = GetComponent<EdgeCollider2D>();
        UpdateCollider();
    }

    void UpdateCollider()
    {
        if (lineRenderer.positionCount < 2)
        {
            Debug.LogWarning("LineRenderer needs at least 2 points to create a collider.");
            return;
        }

        List<Vector2> colliderPoints = new List<Vector2>();

        for (int i = 0; i < lineRenderer.positionCount; i++)
        {
            Vector3 linePoint = lineRenderer.GetPosition(i);
            colliderPoints.Add(new Vector2(linePoint.x, linePoint.y));
        }

        edgeCollider.SetPoints(colliderPoints);
    }

    void Update()
    {
        UpdateCollider(); // Update collider in case the line changes
    }
}
