using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDebug : MonoBehaviour {
    private void OnGUI() {
        GUI.Label(new Rect(10, 10, 1000, 50), $"Webcam: {WebcamMotionCapture.instance.webcamTexture.width} x {WebcamMotionCapture.instance.webcamTexture.height}");
        GUI.Label(new Rect(10, 30, 1000, 50), $"Render: {WebcamMotionCapture.instance.renderTexture.width} x {WebcamMotionCapture.instance.renderTexture.height}");
    }
}
