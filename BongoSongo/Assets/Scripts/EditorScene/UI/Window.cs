using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour {

    public bool isOpened;
    public Button closeButton;
    public GameObject panel;

    public void Open () {
        isOpened = true;
        panel.SetActive(isOpened);
    }
    public void Close() {
        isOpened = false;
        panel.SetActive(isOpened);
    }

    private void OnValidate () {
        panel.SetActive(isOpened);
    }
}
