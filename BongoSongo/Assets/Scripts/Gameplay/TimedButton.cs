using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedButton : MonoBehaviour {
    public Transform indicator;
    public Transform mesh;
    public AudioSource source;

    private float interactTime;
    private Interactable interactable;
    private bool isKilled;

    void Start() {
        interactable = GetComponent<Interactable>();

        interactable.OnInteract += Action;

        interactTime = Time.realtimeSinceStartup + BeatManager.beatLength;
    }

    void Update() {
        if (isKilled) return;

        var time = Time.realtimeSinceStartup;

        // Destroy object if its too late.
        if (time - interactTime > 1f) {
            Destroy(gameObject);
        }

        // Set timing object scale
        mesh.localScale = CameraTransform.Scale(Vector3.one * interactable.size * (2 - Mathf.Clamp((time - interactTime) / BeatManager.beatLength + 1, 0, 2)));

        // Show indicator as red when they're late.
        var redLerp = Mathf.Max((time - interactTime) / BeatManager.beatLength - 1, 0);
        var color = Color.Lerp(Color.white, Color.red, redLerp);

        indicator.GetComponent<Renderer>().material.color = color;
    }

    void Action(object sender, EventArgs eventArgs) {
        var time = Time.realtimeSinceStartup;

        var difference = Mathf.Min(Mathf.Abs(interactTime - time), 1);

        var addScore = Mathf.Abs((interactTime - time) / BeatManager.beatLength);
        var finalScore = 0;

        if (addScore == 0) {
            finalScore = 300;
        } else if (addScore > 0 && addScore < 0.25f) {
            finalScore = 100;
        } else {
            finalScore = 50;
        }

        GameManager.instance.AddScore(finalScore);

        interactable.isActive = false;
        isKilled = true;

        StartCoroutine(Kill());
    }

    IEnumerator Kill() {
        source.Play();

        var i = 0f;

        var lerp = ((Time.realtimeSinceStartup - interactTime) / BeatManager.beatLength + 1) / 2;
        var color = Color.Lerp(new Color(1, 0.5f, 0), Color.green, lerp);

        indicator.GetComponent<Renderer>().material.color = color;

        while (i < 1f) {
            i += Time.deltaTime * 5f;

            indicator.localScale = CameraTransform.Scale(Vector3.one * interactable.size * (1 - i));

            mesh.localScale = CameraTransform.Scale(Vector3.one * interactable.size * i);

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
