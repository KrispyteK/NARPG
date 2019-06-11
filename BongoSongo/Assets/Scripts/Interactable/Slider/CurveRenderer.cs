using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode, RequireComponent(typeof(LineRenderer))]
public class CurveRenderer : MonoBehaviour {

    public Vector3[] points;
    public Color color = Color.white;
    public float width = 0.2f;
    public int numberOfPoints = 20;
    public float tolerance = 0.01f;

    LineRenderer lineRenderer;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }

    void Update() {
        int pointAmt = points.Length * numberOfPoints;

        if (points.Length == 0) {
            lineRenderer.SetPositions(new [] {Vector3.zero});

            return;
        }

        if (pointAmt > 0) {
            lineRenderer.positionCount = pointAmt;
        }

        for (int i = 0; i < pointAmt; i++) {
            lineRenderer.SetPosition(i, Curve.NURBS(points, i / ((float)pointAmt - 1)));
        }

        lineRenderer.Simplify(tolerance);
    }
}