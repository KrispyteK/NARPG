using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(CurveRenderer))]
public class SliderHandles : MonoBehaviour {

    public Transform handleTransform;

    private CurveRenderer curveRenderer;
    private PolygonCollider2D polyCollider;

    void Start() {
        curveRenderer = GetComponent<CurveRenderer>();
        polyCollider = GetComponent<PolygonCollider2D>();
    }

    void Update() {
        var points = new List<Vector3>();
        var points2D = new List<Vector2>();

        Vector2 min = Vector2.zero;
        Vector2 max = Vector2.zero;

        foreach (Transform child in handleTransform) {
            child.GetComponent<SliderHandleSelector>().slider = gameObject;

            points.Add(child.position);
            points2D.Add(child.position);

            if (child.localPosition.x < min.x) min.x = child.localPosition.x - 0.1f;
            if (child.localPosition.y < min.y) min.y = child.localPosition.y - 0.1f;
            if (child.localPosition.x > max.x) max.x = child.localPosition.x + 0.1f;
            if (child.localPosition.y > max.y) max.y = child.localPosition.y + 0.1f;
        }

        curveRenderer.points = points.ToArray();
        polyCollider.points = new Vector2[] {
            min,
            new Vector2(min.x,max.y),
            max,
            new Vector2(max.x,min.y),
            };
    }
}
