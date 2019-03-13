using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebcamMotionCapture : MonoBehaviour
{
    public static WebcamMotionCapture instance;

    public int textureScale = 2;
    public float threshold = 0.1f;
    public float motionDelay = 0.1f;
    public float mipMapBias = 0f;
    public RenderTexture renderTexture;

    private WebCamTexture webcamTexture;
    private Texture2D texCurr;
    private Texture2D texPrev;
    private Renderer renderComponent;
    private bool hasWebcam;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Debug.LogError("Please only place one WebcamMotionCapture object in the scene.");
        }
    }

    private void Start()
    {
        renderComponent = GetComponent<Renderer>();

        webcamTexture = new WebCamTexture();
        webcamTexture.Play();

        StartCoroutine(WebCamStartCoroutine());

        renderComponent.material.SetFloat("_Threshold", threshold);
    }


    private void Update()
    {
        if (!hasWebcam) return;

        if (webcamTexture.didUpdateThisFrame)
        {
            webcamTexture.mipMapBias = mipMapBias;

            var pixels = webcamTexture.GetPixels32();

            texCurr.SetPixels32(pixels);
            texCurr.Apply();

            StartCoroutine(DelayedWebCamCapture(pixels));

            renderComponent.material.SetTexture("_MainTex", texCurr);
        }
    }

    private IEnumerator DelayedWebCamCapture (Color32[] pixels)
    {
        yield return new WaitForSeconds(motionDelay);

        texPrev.SetPixels32(pixels);
        texPrev.Apply();

        renderComponent.material.SetTexture("_PrevTex", texPrev);
    }

    private IEnumerator WebCamStartCoroutine()
    {
        Debug.Log("Waiting for correct webcam info...");

        yield return new WaitWhile(() => webcamTexture.width < 100);

        texCurr = new Texture2D(webcamTexture.width, webcamTexture.height);
        texPrev = new Texture2D(webcamTexture.width, webcamTexture.height);

        renderTexture.width = Camera.main.pixelWidth / 4;
        renderTexture.height = Camera.main.pixelHeight / 4;

        print(renderTexture.width / renderTexture.height);

        transform.localScale = new Vector3(Camera.main.aspect, 1,1);

        hasWebcam = true;
    }
}
