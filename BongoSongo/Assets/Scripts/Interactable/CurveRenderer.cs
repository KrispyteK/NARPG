using UnityEngine;
using System.Collections.Generic;
using System.Linq;

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

    private Vector3 ClosestPointOnLine(Vector3 vA, Vector3 vB, Vector3 vPoint) {
        var vVector1 = vPoint - vA;
        var vVector2 = (vB - vA).normalized;

        var d = Vector3.Distance(vA, vB);
        var t = Vector3.Dot(vVector2, vVector1);

        var vVector3 = vVector2 * t;

        var vClosestPoint = vA + vVector3;

        return vClosestPoint;
    }

    public Vector3 NURBS(Vector3[] points, float t) {
        //function vector nurbs(Array: array, Lerp) {
        //    local Data = array()
        //    local Depth = Array:count()


        //    for (I = 1, Array:count()) {
        //            Data:pushVector(Array[I, vector])
        //    }

        //    while (Depth) {
        //        for (I = 1, Depth - 1) {
        //            local Pos = mix(Data[I, vector], Data[I + 1, vector], Lerp)


        //        Data[I, vector] = Pos
        //        }

        //        Depth--
        //    }

        //    return Data[1, vector]
        //}


        var pointList = new List<Vector3>(points);
        var depth = points.Length;

        while (depth > 0) {
            for (int i = 0; i < depth - 1; i++) {
                var pos = Vector3.Lerp(pointList[i], pointList[i + 1], t);

                pointList[i] = pos;
            }

            depth--;
        }

        return pointList.First();
    }

    private Vector3 TripletNormal (Vector3 p0, Vector3 p1, Vector3 p2) {
        var side1 = p1 - p0;
        var side2 = p2 - p0;
        var perp = Vector3.Cross(side1,side2);

        return perp / perp.magnitude;
    }

    private Vector3[] GenerateCurve (Vector3[] controlPoints) {
        var curve = new List<Vector3>();

        print("----");

        curve.Add(controlPoints[0]);

        for (int i = 1; i < controlPoints.Length - 1; i++) {
            Vector3 before = controlPoints[i - 1];
            Vector3 after = controlPoints[i + 1];
            Vector3 current = controlPoints[i];

            Vector3 beforeDirection = current - before;
            Vector3 afterDirection = after - current;

            Vector3 onLine = (before + after)/2f;
            Vector3 onLineToCurrent = current - onLine;
            Vector3 rotation = Quaternion.AngleAxis(90, TripletNormal(before,current,after)) * onLineToCurrent.normalized;

            float dotbefore = Vector3.Dot(rotation, beforeDirection.normalized);
            float dotafter = Vector3.Dot(rotation, afterDirection.normalized);

            //curve.Add(current - rotation * beforeDirection.magnitude / 2 * dotbefore);
            curve.Add(current);
            curve.Add(current + rotation * afterDirection.magnitude / 2 * dotafter);
        }

        curve.Insert(0, controlPoints[0]);
        curve.Insert(1, controlPoints[0] + (curve[1] - controlPoints[0]));

        curve.Add(controlPoints.Last());
        curve.Insert(0, controlPoints[0]);

        return curve.ToArray();
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

        for (int i = 0; i < numberOfPoints; i++) {
            lineRenderer.SetPosition(i, NURBS(points, i / (float)numberOfPoints));
        }

        //var curve = GenerateCurve(points);

        //for (int i = 0; i < curve.Length - 1; i++) {
        //    Debug.DrawLine(curve[i], curve[i + 1], Color.HSVToRGB(i/ (float)curve.Length,1,1));
        //}

        //for (int i = 0; i < curve.Length; i += 4) {
        //    for (int j = 0; j < 10; i++) {
        //        var _p0 = curve[i];
        //        var _p1 = curve[i + 1];
        //        var _p2 = curve[i + 2];
        //        var _p3 = curve[i + 3];

        //        lineRenderer.SetPosition(j, SolveCubicCurve(_p0, _p1, _p2, _p3, i / 10f));
        //    }
        //}
    }
}