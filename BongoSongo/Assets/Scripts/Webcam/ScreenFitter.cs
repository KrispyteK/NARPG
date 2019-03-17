using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(RawImage))]
[RequireComponent(typeof(AspectRatioFitter))]
public class ScreenFitter : MonoBehaviour
{
    private RawImage rawimage;
    private AspectRatioFitter aspectRatioFitter;

    void Awake()
    {
        rawimage = GetComponent<RawImage>();
        aspectRatioFitter = GetComponent<AspectRatioFitter>();
    }

    void Update ()
    {
        var webcamTexture = WebcamMotionCapture.instance.webcamTexture;

        if (webcamTexture.width < 100) return;

        // change as user rotates iPhone or Android:

        int cwNeeded = webcamTexture.videoRotationAngle;
        // Unity helpfully returns the _clockwise_ twist needed
        // guess nobody at Unity noticed their product works in counterclockwise:
        int ccwNeeded = -cwNeeded;

        // IF the image needs to be mirrored, it seems that it
        // ALSO needs to be spun. Strange: but true.
        if (webcamTexture.videoVerticallyMirrored) ccwNeeded += 180;

        // you'll be using a UI RawImage, so simply spin the RectTransform
        rawimage.rectTransform.localEulerAngles = new Vector3(0f, 0f, ccwNeeded);

        float videoRatio = (float)webcamTexture.width / (float)webcamTexture.height;

        aspectRatioFitter.aspectRatio = videoRatio;

        if (webcamTexture.videoVerticallyMirrored)
            rawimage.uvRect = new Rect(1, 0, -1, 1);  // means flip on vertical axis
        else
            rawimage.uvRect = new Rect(1, 0, -1, 1);  // means no flip

        //Debug.Log("videoRatio: " + videoRatio.ToString());


    }
}
