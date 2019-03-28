using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SlideSegment {
    public Vector2 end;
    public int beats = 1;
}

public class Slide : MonoBehaviour {

    public Vector2 start;
    public int score;
    public SlideSegment[] segments;
    public Interactable interactable;

    private float i;
    private int currentSegmentIndex = -1;
    private SlideSegment currentSegment;
    private Vector2 startingPoint;
    private bool isFinished;

    void Start () {
        NextSegment();

        StartCoroutine(CountScore());
    }

    void Update() {
        if (isFinished) return;
             
        i += Time.deltaTime / (BeatManager.beatLength * currentSegment.beats);

        if (i >= 1) NextSegment();

        var lerp = Vector2.Lerp(startingPoint, currentSegment.end, i);

        transform.position = CameraTransform.ScreenPointToWorld(new Vector2(
                lerp.x,
                lerp.y / Camera.main.aspect
            ));
    }

    void NextSegment () {
        currentSegmentIndex++;

        if (currentSegmentIndex == segments.Length) {
            isFinished = true;

            Destroy(gameObject);

            return;
        }

        startingPoint = currentSegment != null ? currentSegment.end : start;

        currentSegment = segments[currentSegmentIndex];

        i = 0;
    }
    
    IEnumerator CountScore () {
        while (true) {
            yield return new WaitForSeconds(BeatManager.beatLength);

            if (interactable.isInteractedWith) GameManager.instance.score += score;
        }
    }
}
