using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeatText : MonoBehaviour {

    public Transform beatTextTransform;
    public IndicatorInfo indicatorInfo;

    void Start() {
        indicatorInfo = transform.parent.GetComponentInChildren<IndicatorInfo>();
    }

    void Update() {
        var root = indicatorInfo.transform;

        while (root.parent != EditorManager.instance.indicatorParent) {
            root = root.parent;
        }

        var renderers = root.GetComponentsInChildren<Renderer>();
        var bounding = new Bounds(renderers.First().bounds.center, Vector2.zero);

        foreach (var renderer in renderers) {
            bounding.Encapsulate(renderer.bounds);
        }

        var center = (Vector2)Camera.main.WorldToScreenPoint(bounding.center);
        center = new Vector2(center.x, Camera.main.pixelHeight - center.y);

        var extents = bounding.extents / Camera.main.orthographicSize * Camera.main.pixelHeight;

        //GUI.Label(new Rect(center - (Vector2)extents / 2, extents), "" + indicatorInfo.beat, guiStyle);

        beatTextTransform.position = bounding.center;

        beatTextTransform.GetComponent<TMPro.TMP_Text>().text = "" + indicatorInfo.beat;
    }
}
