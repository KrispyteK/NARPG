using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBackground : MonoBehaviour {
    public bool showMotionCapture = false;

    void Start() {
        WebcamMotionCapture.instance.callback += SetTexture;
    }

    void SetTexture() {
        var webcamTexture = WebcamMotionCapture.instance.webcamTexture;
        var motionTexture = WebcamMotionCapture.instance.renderTexture;

        FloatingText.Create(Vector2.zero, "" + webcamTexture.videoRotationAngle, 10f);

        var useTexture = showMotionCapture ? motionTexture : (Texture)webcamTexture;

        GetComponent<Renderer>().material.mainTexture = useTexture;

        var rotationAngle = webcamTexture.videoRotationAngle;
        //var rotationAngle = 270;

        var isTurned = rotationAngle == 90 || rotationAngle == 270;

        var aspect = isTurned ? (float)webcamTexture.height / (float)webcamTexture.width : (float)webcamTexture.width / (float)webcamTexture.height;

        transform.localScale = new Vector3(-1 / aspect, 1, 1);

        var rotation = Quaternion.LookRotation(-Vector3.up, -Vector3.forward) * Quaternion.Euler(Vector3.up * -rotationAngle);

        transform.rotation = rotation;
    }
}
