using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimedButton : MonoBehaviour {
    public Transform mesh;

    private float interactTime;
    private Interactable interactable;

    void Start () {
        interactable = GetComponent<Interactable>();

        interactable.onInteract += Action;

        interactTime = Time.realtimeSinceStartup + BeatManager.beatLength;
    }

    void Update () {
        var time = Time.realtimeSinceStartup;

        if (time - interactTime > 1f) {
            Destroy(gameObject);
        }

        mesh.localScale = CameraTransform.Scale(Vector3.one * interactable.size * (2 - Mathf.Clamp((time - interactTime) / BeatManager.beatLength + 1, 0,2)));
    }

    void Action () {
        var time = Time.realtimeSinceStartup;

        var difference = Mathf.Min(Mathf.Abs(interactTime - time),1);

        GameManager.instance.AddScore((int)((1 - difference) * 10));

        Destroy(gameObject);
    }
}
