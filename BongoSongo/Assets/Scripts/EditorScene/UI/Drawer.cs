using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Drawer : MonoBehaviour {

    public Sprite openSprite;
    public Sprite closeSprite;

    public bool isOpened;

    public Vector2 closedMin;
    public Vector2 closedMax;
    public Vector2 openedMin;
    public Vector2 openedMax;

    public float time = 0.15f;

    public Button button;

    private float t;
    private RectTransform rectTransform;

    public void Toggle () {
        isOpened = !isOpened;

        if (isOpened) {
            DrawerManager.instance.CloseAllBut(this);
        }

        button.GetComponent<Image>().sprite = !isOpened ? openSprite : closeSprite;
    }

    private void Start() {
        rectTransform = GetComponent<RectTransform>();

        button.onClick.AddListener(Toggle);
    }

    private void OnValidate () {
        rectTransform = GetComponent<RectTransform>();

        rectTransform.anchorMin = Vector2.Lerp(closedMin, openedMin, (isOpened ? 1 : 0));
        rectTransform.anchorMax = Vector2.Lerp(closedMax, openedMax, (isOpened ? 1 : 0));
    }

    private void Update () {
        t += ((isOpened ? 1 : 0) - t) * Time.deltaTime / time;

        rectTransform.anchorMin = Vector2.Lerp(closedMin, openedMin, t);
        rectTransform.anchorMax = Vector2.Lerp(closedMax, openedMax, t);
    }
}
