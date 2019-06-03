using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollRectSnap : MonoBehaviour {

    public float velocityShiftThreshold = 1000f;
    public float velocityShiftThresholdMax = 2000f;
    public float snapThreshold = 5f;
    public float force = 1f;
    public RectTransform panel;
    public RectTransform center;
    public RectTransform[] buttons;

    public RectTransform Selected => buttons[minButtonNum];

    private float[] distances;
    private bool dragging;
    private int buttonDistance;
    private int minButtonNum;
    private ScrollRect scrollRect;
    private bool goToClosest = true;

    void Start () {
        distances = new float[buttons.Length];

        buttonDistance = (int)Mathf.Abs(
            buttons[1].GetComponent<RectTransform>().anchoredPosition.x -
            buttons[0].GetComponent<RectTransform>().anchoredPosition.x
            );

        scrollRect = GetComponent<ScrollRect>();
    }

    void Update () {
        if (goToClosest) {
            for (int i = 0; i < buttons.Length; i++) {
                distances[i] = Mathf.Abs(center.transform.position.x - buttons[i].transform.position.x);
            }

            float minDistance = Mathf.Min(distances);

            for (int a = 0; a < buttons.Length; a++) {
                if (minDistance == distances[a]) {
                    minButtonNum = a;
                    break;
                }
            }
        }

        if (!dragging) {
            LerpToButton(minButtonNum);
        }
    }

    private void LerpToButton(int button) {
        int position = button * -buttonDistance;

        float delta = position - panel.anchoredPosition.x;
        float newX = position;

        if (Mathf.Abs(delta) > snapThreshold) {
            scrollRect.velocity += new Vector2(delta * buttonDistance * Time.deltaTime * force,0);
        } else {
            Vector2 newPosition = new Vector2(newX, panel.anchoredPosition.y);

            panel.anchoredPosition = newPosition;
            scrollRect.velocity = Vector2.zero;
        }
    }

    public void StartDrag () {
        dragging = true;
        goToClosest = true;
    }

    public void EndDrag () {
        dragging = false;

        float velX = scrollRect.velocity.x;

        if (Mathf.Abs(scrollRect.velocity.x) > velocityShiftThresholdMax) {
            minButtonNum = (int)Mathf.Clamp(minButtonNum - Mathf.Sign(velX) * buttons.Length, 0, buttons.Length - 1);

            goToClosest = false;
        } else if (Mathf.Abs(scrollRect.velocity.x) > velocityShiftThreshold && Mathf.Abs(scrollRect.velocity.x) < velocityShiftThresholdMax) {
            minButtonNum = (int)Mathf.Clamp(minButtonNum - Mathf.Sign(velX),0, buttons.Length - 1);

            goToClosest = false;
        }
    }
}
