using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedButton : MonoBehaviour {
    public Transform indicator;
    public Transform ring;
    public GameObject beatScore;
    public AudioSource source;
    public GameObject particleEffect;

    private Interactable interactable;
    private bool isKilled;
    private float timeToInteract;
    private float spawnTime;

    void Start() {
        interactable = GetComponent<Interactable>();
        interactable.size = InterScene.Instance.gamePlaySettings.indicatorScale;
        interactable.OnInteract += Action;

        timeToInteract = BeatManager.beatLength * InterScene.Instance.gamePlaySettings.indicatorBeatLife;

        spawnTime = Time.realtimeSinceStartup;

        ring.localScale = CameraTransform.Scale(Vector3.one * interactable.size * 1);
        indicator.localScale = CameraTransform.Scale(Vector3.one * interactable.size);
    }

    void Update() {
        if (isKilled) return;

        //indicator.localScale = CameraTransform.Scale(Vector3.one * interactable.size);

        var time = Time.realtimeSinceStartup;

        // Destroy object if its too late.
        if (time - spawnTime > 1f) {
            Destroy(gameObject);
        }

        // Set timing object scale
        //ring.localScale = CameraTransform.Scale(Vector3.one * interactable.size * (3 - Mathf.Clamp((time - interactTime) / BeatManager.beatLength + 1, 0, 1) * 2));

        // Show indicator as red when they're late.
        var redLerp = Mathf.Max((time - spawnTime) / timeToInteract - 1, 0);
        var color = Color.Lerp(Color.white, Color.red, redLerp);

        indicator.GetComponent<Renderer>().material.color = color * new Color(1,1,1,0.8f);
    }

    void Action(object sender, EventArgs eventArgs) {
        var time = Time.realtimeSinceStartup;

        var difference = Mathf.Min((time - spawnTime), 1);

        var addScore = Mathf.Abs((time - spawnTime) / BeatManager.beatLength);
        var finalScore = 0;

        //if (addScore > 0.0f && addScore < 0.05f) {
        //    finalScore = 300;
        //} else if (addScore > 0.05f && addScore < 0.25f) {
        //    finalScore = 100;
        //} else {
        //    finalScore = 50;
        //}

        var hitTime = (time - spawnTime);

        if (hitTime < InterScene.Instance.gamePlaySettings.indicatorLateTime) {
            finalScore = InterScene.Instance.gamePlaySettings.indicatorScoreOnTime;
        }
        else {
            finalScore = InterScene.Instance.gamePlaySettings.indicatorScoreOnLate;
        }

        var beatScoreInstance = Instantiate(beatScore, transform.position, Quaternion.identity);
        beatScoreInstance.GetComponent<BeatScore>().Init(finalScore);

        GameManager.instance.AddScore(finalScore);

        interactable.isActive = false;
        isKilled = true;

        StartCoroutine(Kill());
    }

    IEnumerator Kill() {
        source.Play();

        var i = 0f;

        var lerp = ((Time.realtimeSinceStartup - spawnTime) / BeatManager.beatLength + 1) / 2;
        var color = Color.Lerp(new Color(1, 0.5f, 0), Color.green, lerp);

        indicator.GetComponent<Renderer>().material.color = color * new Color(1, 1, 1, 0.8f);

        Instantiate(particleEffect, transform.position, Quaternion.identity);

        while (i < 1f) {
            i += Time.deltaTime * 5f;

            indicator.localScale = CameraTransform.Scale(Vector3.one * interactable.size * (1 - i));

            ring.localScale = CameraTransform.Scale(Vector3.one * interactable.size * (i * 2 + 1));

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
