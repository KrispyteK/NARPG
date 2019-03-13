using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
public class ShowWebcam : MonoBehaviour
{

    private RawImage image;

    void Start()
    {
        image = GetComponent<RawImage>();
        WebcamMotionCapture.instance.callback += OnWebcamStart;
    }

    void OnWebcamStart ()
    {
        image.texture = WebcamMotionCapture.instance.webcamTexture;
    }
}
