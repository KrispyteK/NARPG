using System.Collections.Generic;
using System.Linq;
using UnityEngine;

class Curve {
    public static Vector3 NURBS(Vector3[] points, float t) {
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
}
