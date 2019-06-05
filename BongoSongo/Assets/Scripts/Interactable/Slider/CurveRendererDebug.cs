using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode, RequireComponent(typeof(CurveRenderer))]
public class CurveRendererDebug : MonoBehaviour {

    public Transform debugTransform;

    private SliderIndicator sliderIndicator;

    void Start() {
        sliderIndicator = GetComponent<SliderIndicator>();
    }

    void Update() {
        var points = new List<Vector3>();

        foreach (Transform child in debugTransform) {
            points.Add(child.position);
        }

        sliderIndicator.points = points.ToArray();
    }
}
