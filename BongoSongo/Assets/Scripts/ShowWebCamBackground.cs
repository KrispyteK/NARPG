using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowWebCamBackground : MonoBehaviour {
    void Start() {
        WebcamMotionCapture.instance.callback += SetTexture;
    }

    void SetTexture() {
        var webcamTexture = WebcamMotionCapture.instance.webcamTexture;

        GetComponent<Renderer>().material.mainTexture = webcamTexture;

        var aspect = (float)webcamTexture.width / (float)webcamTexture.height;

        transform.localScale = new Vector3(-1 / aspect, 1, 1);
    }
}
