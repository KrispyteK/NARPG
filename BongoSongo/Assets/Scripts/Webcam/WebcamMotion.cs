using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebcamMotion : MonoBehaviour
{
    void Start()
    {
        var webcamTexture = new WebCamTexture();
        var rawImage = GetComponent<RawImage>();
        rawImage.texture = webcamTexture;
        rawImage.rectTransform.sizeDelta = new Vector2(webcamTexture.width, webcamTexture.height);
        webcamTexture.Play();
    }
}
