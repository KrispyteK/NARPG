using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[ExecuteInEditMode, RequireComponent(typeof(LineRenderer))]
public class CurveRenderer : MonoBehaviour {

    public Vector3[] points;
    public Color color = Color.white;
    public float width = 0.2f;
    public int numberOfPoints = 20;

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
        if (points.Length == 0) {
            lineRenderer.SetPositions(new [] {Vector3.zero});

            return;
        }

        if (numberOfPoints > 0) {
            lineRenderer.positionCount = numberOfPoints;
        }

        for (int i = 0; i < numberOfPoints; i++) {
            lineRenderer.SetPosition(i, Curve.NURBS(points, i / ((float)numberOfPoints - 1)));
        }
    }
}