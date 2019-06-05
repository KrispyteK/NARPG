using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(LineRenderer))]
public class CurveRenderer : MonoBehaviour {

    public Vector3[] points;
    public GameObject start, middle, middle2, end;
    public Color color = Color.white;
    public float width = 0.2f;
    public int numberOfPoints = 20;
    LineRenderer lineRenderer;

    void Start() {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.useWorldSpace = true;
        lineRenderer.material = new Material(Shader.Find("Particles/Additive"));
    }

    private Vector3 SolveCubicCurve (Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
        return
            Mathf.Pow(1 - t, 3) * p0 +
            3 * Mathf.Pow(1 - t, 2) * t * p1 +
            3 * (1 - t) * Mathf.Pow(t, 2) * p2 +
            Mathf.Pow(t, 3) * p3
            ;
    }

    private Vector3[] GenerateCurve (Vector3[] controlPoints) {
        Vector3[] curve = new Vector3[(controlPoints.Length - 1) * 3 + 1];

        print("----");

        curve[0] = controlPoints[0];

        for (int i = 1; i < controlPoints.Length - 1; i++) {
            Vector3 before = controlPoints[i - 1];
            Vector3 after = controlPoints[i + 1];
            Vector3 current = controlPoints[i];

            Vector3 beforeDirection = current - before;
            Vector3 afterDirection = after - current;

            Vector3 middleDirection = (beforeDirection + afterDirection).normalized;

            int curveIndex = i * 3;
            print(curveIndex);

            curve[curveIndex - 1] = current - middleDirection * beforeDirection.magnitude / 2;
            curve[curveIndex] = current;
            curve[curveIndex + 1] = current + middleDirection * afterDirection.magnitude / 2;
        }

        curve[1] = controlPoints[0] + (curve[2] - controlPoints[0]) / 2f;

        curve[curve.Length - 2] = controlPoints[controlPoints.Length - 1] + (curve[curve.Length - 3] - controlPoints[controlPoints.Length - 1]) / 2f;
        curve[curve.Length - 1] = controlPoints[controlPoints.Length - 1];

        return curve;
    }

    void Update() {
        if (lineRenderer == null || start == null || middle == null || end == null) {
            return; // no points specified
        }

        // update line renderer
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;

        if (numberOfPoints > 0) {
            lineRenderer.positionCount = numberOfPoints;
        }

        // set points of quadratic Bezier curve
        Vector3 p0 = start.transform.position;
        Vector3 p1 = middle.transform.position;
        Vector3 p2 = middle2.transform.position;
        Vector3 p3 = end.transform.position;

        points = new Vector3[] { p0, p1, p2, p3 };

        //for (int i = 0; i < numberOfPoints; i++) {
        //    lineRenderer.SetPosition(i, SolveCubicCurve(p0, p1, p2, p3, i / (float)numberOfPoints));
        //}

        var curve = GenerateCurve(points);

        for (int i = 0; i < curve.Length - 2; i++) {
            Debug.DrawLine(curve[i], curve[i + 1], Color.HSVToRGB(i/ (float)curve.Length,1,1));
        }

        for (int i = 0; i < curve.Length; i += 4) {
            for (int j = 0; j < 10; i++) {
                var _p0 = curve[i];
                var _p1 = curve[i + 1];
                var _p2 = curve[i + 2];
                var _p3 = curve[i + 3];

                lineRenderer.SetPosition(j, SolveCubicCurve(_p0, _p1, _p2, _p3, i / 10f));
            }
        }
    }
}