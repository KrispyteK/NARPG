using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WebcamMotionCapture : MonoBehaviour {
    public static WebcamMotionCapture instance;

    public float threshold = 0.1f;
    public float motionDelay = 0.1f;
    public RenderTexture renderTexture;
    public WebCamTexture webcamTexture;
    public Texture2D texture;
    public bool hasWebcam;
    public Action callback;

    private Texture2D texCurr;
    private Texture2D texPrev;
    private Renderer renderComponent;

    private void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Debug.LogError("Please only place one WebcamMotionCapture object in the scene.");
        }
    }

    private void Start() {
        renderComponent = GetComponent<Renderer>();

        string cameraName = null;

        foreach (var camera in WebCamTexture.devices) {
            print(camera.name);

            if (camera.isFrontFacing) {
                cameraName = camera.name;
                break;
            }
        }

        if (cameraName == null) {
            Debug.LogError("Could not find suitable camera device!");
        }

        StartCoroutine(WebCamStartCoroutine(cameraName));

        renderComponent.material.SetFloat("_Threshold", threshold);
    }


    private void Update() {
        if (!hasWebcam) return;

        if (webcamTexture.didUpdateThisFrame) {
            var pixels = webcamTexture.GetPixels32();

            texCurr.SetPixels32(pixels);
            texCurr.Apply();

            // Capture old pixels.
            StartCoroutine(DelayedWebCamCapture(pixels));

            renderComponent.material.SetTexture("_MainTex", texCurr);

            // Render renderTexture to Texture2D.
            RenderTexture.active = renderTexture;

            texture.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);

            RenderTexture.active = null;
        }
    }

    private IEnumerator DelayedWebCamCapture(Color32[] pixels) {
        yield return new WaitForSeconds(motionDelay);

        texPrev.SetPixels32(pixels);
        texPrev.Apply();

        renderComponent.material.SetTexture("_PrevTex", texPrev);
    }

    private IEnumerator WebCamStartCoroutine(string cameraName) {

        webcamTexture = new WebCamTexture(cameraName);
        webcamTexture.Play();

        Debug.Log("Waiting for correct webcam info...");

        yield return new WaitWhile(() => {
            return webcamTexture.width == 16;
        });

        var ccwNeeded = 0f;

        switch (webcamTexture.videoRotationAngle) {
            case 270:
                ccwNeeded = -90f;
                break;
        }

        //if (webcamTexture.videoVerticallyMirrored) ccwNeeded += 180;

        texCurr = new Texture2D(webcamTexture.width, webcamTexture.height);
        texPrev = new Texture2D(webcamTexture.width, webcamTexture.height);
        texture = new Texture2D(renderTexture.width, renderTexture.height);

        var aspectRatioTexture = (float)webcamTexture.width / (float)webcamTexture.height;

        var motionCaptureCamera = GameObject.FindGameObjectWithTag("MotionCaptureCamera").GetComponent<Camera>();
        motionCaptureCamera.rect = new Rect(0, 0, aspectRatioTexture, 1);

        renderTexture.height = (int)(256 * aspectRatioTexture);

        transform.localScale = new Vector3(aspectRatioTexture, 1, 1);

        transform.rotation = Quaternion.Euler(0,ccwNeeded,0);

        hasWebcam = true;

        // Call callback function if there is any.
        callback?.Invoke();

        Debug.Log("Got correct webcam info!");
    }
}
