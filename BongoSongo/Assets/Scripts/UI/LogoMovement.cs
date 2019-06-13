using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogoMovement : MonoBehaviour {

    public float bpm = 128;
    public float scaleMin = 0.8f;
    public float scaleMax = 1.0f;
    public float angle = 10f;
    public float angleSpeed = 0.25f;

    private RectTransform rectTransform;
    private float t;

    void Start() {
        rectTransform = GetComponent<RectTransform>();

        var beatManager = FindObjectOfType<BeatManager>();

        if (beatManager) {
            bpm = beatManager.bpm;
        }
    }

    void Update() {
        var time = (60 / bpm);

        t += Time.deltaTime / time;

        var i = (1 - (t - Mathf.Floor(t)));

        rectTransform.localScale = Vector3.one * Mathf.Lerp(scaleMin, scaleMax, i);

        rectTransform.rotation = Quaternion.Euler(0,0,(Mathf.PerlinNoise(t * angleSpeed, t * angleSpeed + 1000f) * 2 - 1) * angle);
    }
}
