using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(CurveRenderer))]
public class SliderHandles : MonoBehaviour {

    public Transform handleTransform;

    private CurveRenderer curveRenderer;
    private BoxCollider2D boxCollider;

    void Start() {
        curveRenderer = GetComponent<CurveRenderer>();
        boxCollider = GetComponent<BoxCollider2D>();
    }

    void Update() {
        var points = new List<Vector3>();

        var bounds = new Bounds(handleTransform.GetChild(0).position, Vector3.zero);

        foreach (Transform child in handleTransform) {
            //child.GetComponent<SliderHandleSelector>().slider = gameObject;

            points.Add(child.position);

            bounds.Encapsulate(child.position);
        }

        curveRenderer.points = points.ToArray();
        boxCollider.size = bounds.size;
    }
}
