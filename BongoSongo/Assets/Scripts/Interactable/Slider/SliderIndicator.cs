using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderIndicator : MonoBehaviour {

    public Transform indicator;
    public int beats;
    public Vector3[] points;
    public float t;
    public int direction = 1;

    private CurveRenderer curveRenderer;
    private Interactable interactable;

    void Start() {
        interactable = indicator.GetComponent<Interactable>();

        curveRenderer = GetComponent<CurveRenderer>();

        indicator.localScale = CameraTransform.Scale(Vector3.one * interactable.size * 0.5f);

        InvokeRepeating("BeatEvent", 0f, BeatManager.beatLength);
    }

    void BeatEvent () {
        if (interactable.isInteractedWith) {
            FloatingText.Create(indicator.position, $"+{InterScene.Instance.gamePlaySettings.sliderScore}", BeatManager.beatLength * 2f);

            GameManager.instance.AddScore(InterScene.Instance.gamePlaySettings.sliderScore);
        }
    }

    void Update() {
        curveRenderer.points = points;

        t = Mathf.Clamp01(t + Time.deltaTime / BeatManager.beatLength / beats * direction);

        if (t == 1) {
            direction = -1;
        } else if (t == 0) {
            direction = 1;
        }

        var pos = Curve.NURBS(points, t);

        indicator.position = Curve.NURBS(points, t);

        var tangent = (pos - Curve.NURBS(points, t - 0.001f)).normalized;

        float rot_z = Mathf.Atan2(tangent.y, tangent.x) * Mathf.Rad2Deg;
        indicator.rotation = Quaternion.Euler(0f, 0f, rot_z + 180 * (1-(direction + 1)/2));
    }
}
