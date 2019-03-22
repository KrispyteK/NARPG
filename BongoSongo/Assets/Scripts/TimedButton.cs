using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedButton : MonoBehaviour {
    public RawImage rawImage;

    private float interactTime;
    private Interactable interactable;

    void Start () {
        interactable = GetComponent<Interactable>();

        interactable.onInteract += Action;

        interactTime = Time.realtimeSinceStartup + 4f * BeatManager.beatLength;
    }

    void Update () {
        if (Time.realtimeSinceStartup - interactTime > 1f) {
            Destroy(gameObject);
        }

        rawImage.rectTransform.sizeDelta = new Vector2(interactable.size * Camera.main.pixelWidth, interactable.size * Camera.main.pixelWidth) * Mathf.Min(Mathf.Abs(interactTime - Time.realtimeSinceStartup), 0);
    }

    void Action () {
        var time = Time.realtimeSinceStartup;

        var difference = Mathf.Min(Mathf.Abs(interactTime - time),1);

        GameManager.instance.AddScore((int)((1 - difference) * 10));

        Destroy(gameObject);
    }
}
