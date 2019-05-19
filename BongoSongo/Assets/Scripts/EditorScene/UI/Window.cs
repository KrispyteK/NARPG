using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Window : MonoBehaviour {

    public bool isOpened;

    public GameObject panel;

    private void OnValidate () {
        panel.SetActive(isOpened);
    }
}
