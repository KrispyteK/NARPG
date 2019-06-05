using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderIndicator : MonoBehaviour {

    public Transform indicator;
    public int beats;
    public Vector3[] points;
    public float t;
    public int direction = 1;
    public int score = 100;

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
            FloatingText.Create(indicator.position, $"+{score}", BeatManager.beatLength * 2f);

            GameManager.instance.AddScore(score);
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

        indicator.position = Curve.NURBS(points, t);
    }
}
