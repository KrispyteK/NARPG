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
    public float damping = 0.05f;
    public RectTransform panel;
    public RectTransform center;
    public RectTransform[] buttons;
    public int index;
    public int buttonDistance;

    public float DistanceToNearest => (index * -buttonDistance) - panel.anchoredPosition.x;

    public RectTransform Selected => buttons[index];

    private float[] distances;
    private bool dragging;
    private ScrollRect scrollRect;
    private bool goToClosest = true;

    void Start () {
        SetButtons(buttons);

        scrollRect = GetComponent<ScrollRect>();
    }

    public void GoToIndex(int i) {
        goToClosest = false;
        index = i;
    }

    public void SetButtons (RectTransform[] btns) {
        buttons = btns;

        distances = new float[buttons.Length];

        buttonDistance = (int)Mathf.Abs(
            buttons[1].GetComponent<RectTransform>().anchoredPosition.x -
            buttons[0].GetComponent<RectTransform>().anchoredPosition.x
            );
    }

    void Update () {
        if (goToClosest) {
            for (int i = 0; i < buttons.Length; i++) {
                distances[i] = Mathf.Abs(center.transform.position.x - buttons[i].transform.position.x);
            }

            float minDistance = Mathf.Min(distances);

            for (int a = 0; a < buttons.Length; a++) {
                if (minDistance == distances[a]) {
                    index = a;
                    break;
                }
            }
        }

        if (!dragging) {
            LerpToButton(index);
        }
    }

    private void LerpToButton(int button) {
        int position = button * -buttonDistance;

        float delta = position - panel.anchoredPosition.x;
        float newX = position;

        if (Mathf.Abs(delta) > snapThreshold) {
            scrollRect.velocity += new Vector2(delta * buttonDistance * Time.deltaTime * force,0) - scrollRect.velocity * damping * Time.deltaTime;
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
            index = (int)Mathf.Clamp(index - Mathf.Sign(velX) * buttons.Length, 0, buttons.Length - 1);

            goToClosest = false;
        } else if (Mathf.Abs(scrollRect.velocity.x) > velocityShiftThreshold && Mathf.Abs(scrollRect.velocity.x) < velocityShiftThresholdMax) {
            index = (int)Mathf.Clamp(index - Mathf.Sign(velX),0, buttons.Length - 1);

            goToClosest = false;
        }
    }
}
