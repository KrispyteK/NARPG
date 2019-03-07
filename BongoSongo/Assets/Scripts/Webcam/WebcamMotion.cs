using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Collections;

public struct PixelJob : IJobParallelFor
{
    [NativeDisableParallelForRestriction]  public NativeArray<Color> pixels;
    [NativeDisableParallelForRestriction]  public NativeArray<Color> oldValues;
    [NativeDisableParallelForRestriction]  public NativeArray<Color> newPixels;
    public int srcWidth;
    public int srcHeight;
    public int tgtWidth;
    public int tgtHeight;
    public int scale;
    public float threshold;

    public void Execute(int i)
    {
        var x = i % tgtWidth;
        var y = (i - x) / tgtWidth;

        var xScaled = scale * x;
        var yScaled = scale * y;

        var p = Color.Lerp(pixels[yScaled * srcWidth + xScaled], pixels[yScaled * srcWidth + xScaled + 1], 0.5f);
        var q = Color.Lerp(pixels[(yScaled + 1) * srcWidth + xScaled], pixels[(yScaled + 1) * srcWidth + xScaled + 1], 0.5f);

        var color = Color.Lerp(p, q, 0.5f);
        var oldColor = oldValues[y * tgtWidth + x];

        var oldGrayScale = (oldColor.r + oldColor.g + oldColor.b) / 3;
        var grayScale = (color.r + color.g + color.b) / 3;

        var difference = Mathf.Abs(grayScale - oldGrayScale);
        var hasMotion = difference > threshold;
        var displayColor = hasMotion ? Color.red : Color.black;

        newPixels[y * tgtWidth + x] = displayColor;
        oldValues[y * tgtWidth + x] = color;
    }
}

public class WebcamMotion : MonoBehaviour
{
    public int textureScale = 2;
    public float threshold = 0.5f;

    private int appliedScale => 1 << textureScale;
    private WebCamTexture webcamTexture;
    private RawImage rawImage;
    private AspectRatioFitter aspectRatioFitter;
    private Color[] previousPixelValues;
    private float[] previousPixelValuesFloat;
    private Texture2D scaledDownTexture;
    private bool hasWebcam;

    private void Start()
    {
        webcamTexture = new WebCamTexture();
        aspectRatioFitter = GetComponent<AspectRatioFitter>();

        rawImage = GetComponent<RawImage>();
        rawImage.texture = webcamTexture;
        webcamTexture.Play();

        StartCoroutine(WebCamStartCoroutine());
    }

    private void Update() {
        if (!hasWebcam) return;

        //SetTextureJobs();

        SetTexture();
    }

    private void SetTextureJobs ()
    {
        var pixels = webcamTexture.GetPixels();
        var pixelArray = new NativeArray<Color>(pixels.Length, Allocator.Persistent);
        var newPixelArray = new NativeArray<Color>(scaledDownTexture.width * scaledDownTexture.height, Allocator.Persistent);
        var oldValues = new NativeArray<Color>(previousPixelValues.Length,Allocator.Persistent);

        for (int i = 0; i < pixelArray.Length; i++) pixelArray[i] = pixels[i];
        for (int i = 0; i < oldValues.Length; i++) oldValues[i] = previousPixelValues[i];

        var job = new PixelJob
        {
            srcHeight = webcamTexture.height,
            srcWidth = webcamTexture.width,
            tgtHeight = webcamTexture.height / appliedScale,
            tgtWidth = webcamTexture.width / appliedScale,
            scale = appliedScale,
            threshold = threshold,
            pixels = pixelArray,
            oldValues = oldValues,
            newPixels = newPixelArray
        };

        var jobHandle = job.Schedule(newPixelArray.Length, 32);

        jobHandle.Complete();


        previousPixelValues = oldValues.ToArray();

        scaledDownTexture.SetPixels(newPixelArray.ToArray());
        scaledDownTexture.Apply();
        rawImage.texture = scaledDownTexture;

        pixelArray.Dispose();
        newPixelArray.Dispose();
        oldValues.Dispose();
    }

    private void SetTexture()
    {
        var pixels = webcamTexture.GetPixels();
        var targetPixels = new Color[scaledDownTexture.width * scaledDownTexture.height];

        if (previousPixelValuesFloat == null) previousPixelValuesFloat = new float[targetPixels.Length];

        var srcWidth = webcamTexture.width;
        var srcHeight = webcamTexture.height;

        var tgtWidth = srcWidth / appliedScale;
        var tgtHeight = srcHeight / appliedScale;

        for (var y = 0; y < tgtHeight; y++)
        {
            var yScaled = appliedScale * y;

            for (var x = 0; x < tgtWidth; x++)
            {
                var xScaled = appliedScale * x;

                var p = Color.Lerp(pixels[yScaled * srcWidth + xScaled], pixels[yScaled * srcWidth + xScaled + 1], 0.5f);
                var q = Color.Lerp(pixels[(yScaled + 1) * srcWidth + xScaled], pixels[(yScaled + 1) * srcWidth + xScaled + 1], 0.5f);

                var color = Color.Lerp(p, q, 0.5f);

                //var color = pixels[yScaled * srcWidth + xScaled];
                var oldValue = previousPixelValuesFloat[y * tgtWidth + x];
                var grayScale = (color.r + color.g + color.b) / 3;

                var difference = Mathf.Abs(grayScale - oldValue);
                var hasMotion = difference > threshold;
                var displayColor = hasMotion ? Color.red : Color.black;

                targetPixels[y * tgtWidth + x] = displayColor;
                previousPixelValuesFloat[y * tgtWidth + x] = grayScale;
            }
        }

        scaledDownTexture.SetPixels(targetPixels);
        scaledDownTexture.Apply();

        rawImage.texture = scaledDownTexture;
    }

    private IEnumerator WebCamStartCoroutine() {
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
            rawImage.uvRect = new Rect(1, 0, -1, 1);  // means no flip

        // devText.text =
        //  videoRotationAngle+"/"+ratio+"/"+wct.videoVerticallyMirrored;

        scaledDownTexture = new Texture2D(webcamTexture.width / appliedScale, webcamTexture.height / appliedScale);
        scaledDownTexture.filterMode = FilterMode.Point;

        previousPixelValues = new Color[scaledDownTexture.width * scaledDownTexture.height];

        hasWebcam = true;
    }
}
