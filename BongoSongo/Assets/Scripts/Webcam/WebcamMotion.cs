using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WebcamMotion : MonoBehaviour
{
    WebCamTexture webcamTexture;
    RawImage rawImage;
    AspectRatioFitter aspectRatioFitter;

    void Start()
    {
        webcamTexture = new WebCamTexture();
        aspectRatioFitter = GetComponent<AspectRatioFitter>();
        rawImage = GetComponent<RawImage>();
        rawImage.texture = webcamTexture;
        rawImage.rectTransform.sizeDelta = new Vector2(webcamTexture.width, webcamTexture.height);
        webcamTexture.Play();

        StartCoroutine(WebCamStartCoroutine());
    }

    void Update() {

    }

    IEnumerator WebCamStartCoroutine() {
        Debug.Log("Waiting for correct webcam info...");

        yield return new WaitWhile (() => webcamTexture.width < 100);

        // change as user rotates iPhone or Android:

        int cwNeeded = webcamTexture.videoRotationAngle;
        // Unity helpfully returns the _clockwise_ twist needed
        // guess nobody at Unity noticed their product works in counterclockwise:
        int ccwNeeded = -cwNeeded;

        // IF the image needs to be mirrored, it seems that it
        // ALSO needs to be spun. Strange: but true.
        if (webcamTexture.videoVerticallyMirrored) ccwNeeded += 180;

        // you'll be using a UI RawImage, so simply spin the RectTransform
        rawImage.rectTransform.localEulerAngles = new Vector3(0f, 0f, ccwNeeded);

        float videoRatio = (float)webcamTexture.width / (float)webcamTexture.height;

        // you'll be using an AspectRatioFitter on the Image, so simply set it
        aspectRatioFitter.aspectRatio = videoRatio;

        // alert, the ONLY way to mirror a RAW image, is, the uvRect.
        // changing the scale is completely broken.
        if (webcamTexture.videoVerticallyMirrored)
            rawImage.uvRect = new Rect(1, 0, -1, 1);  // means flip on vertical axis
        else
            rawImage.uvRect = new Rect(0, 0, 1, 1);  // means no flip

        // devText.text =
        //  videoRotationAngle+"/"+ratio+"/"+wct.videoVerticallyMirrored;
    }
}
