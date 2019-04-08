using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraDebug : MonoBehaviour {
    public GUIStyle style;

    private void OnGUI() {

        var textSize = Camera.main.pixelHeight * 0.04f;
        style.fontSize = (int)textSize;

        if (WebcamMotionCapture.instance.webcamTexture) {
            GUI.Label(new Rect(10, 10, 1000, 50), $"Webcam: {WebcamMotionCapture.instance.webcamTexture.width} x {WebcamMotionCapture.instance.webcamTexture.height}", style);
            GUI.Label(new Rect(10, 10 + textSize, 1000, 50), $"Render: {WebcamMotionCapture.instance.renderTexture.width} x {WebcamMotionCapture.instance.renderTexture.height}", style);
        }

        var interactables = FindObjectsOfType<Interactable>();

        foreach (var interactable in interactables) {
            var screenPoint = Camera.main.WorldToScreenPoint(interactable.transform.position);

            GUI.Label(new Rect(screenPoint.x, Camera.main.pixelHeight - screenPoint.y + Camera.main.pixelHeight * interactable.size / 2, 100, 50), "" + interactable.interactionAmount, style);
        }
    }
}
