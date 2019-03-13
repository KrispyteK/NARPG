using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebcamMotionCapture : MonoBehaviour
{
    public int textureScale = 2;
    public float threshold = 0.5f;

    private int appliedScale => 1 << textureScale;
    private WebCamTexture webcamTexture;
    private Texture2D texCurr;
    private Texture2D texPrev;
    private Renderer renderer;
    private bool hasWebcam;

    private void Start()
    {
        renderer = GetComponent<Renderer>();

        webcamTexture = new WebCamTexture();
        webcamTexture.Play();

        StartCoroutine(WebCamStartCoroutine());
    }


    private void FixedUpdate()
    {
        if (!hasWebcam) return;

        var pixels = webcamTexture.GetPixels32();

        texCurr.SetPixels32(pixels);
        texCurr.Apply();

        renderer.material.SetTexture("_MainTex", texCurr);
        renderer.material.SetTexture("_PrevTex", texPrev);

        StartCoroutine(DelayedWebCamCapture());
    }

    private IEnumerator DelayedWebCamCapture ()
    {
        yield return new WaitForSeconds(Time.fixedDeltaTime);

        texPrev.SetPixels32(webcamTexture.GetPixels32());
        texPrev.Apply();
    }

    private IEnumerator WebCamStartCoroutine()
    {
        Debug.Log("Waiting for correct webcam info...");

        yield return new WaitWhile(() => webcamTexture.width < 100);     

        texCurr = new Texture2D(webcamTexture.width, webcamTexture.height);
        texPrev = new Texture2D(webcamTexture.width, webcamTexture.height);

        hasWebcam = true;
    }
}
