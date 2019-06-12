using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TimelineDrawer : MonoBehaviour, IPointerClickHandler {
    public RectTransform timeIndicator;
    public RectTransform image;
    public ScrollRect scrollRect;
    public int maxZoom = 50;
    public float defaultScrollSensitivity;

    private Texture2D texture;
    private int zoom = 0;
    private Vector2 defaultSize;

    void Start() {
        defaultSize = (transform.parent as RectTransform).sizeDelta;
    }
    void Update() {
        scrollRect.scrollSensitivity = Input.GetKey(KeyCode.LeftControl) ? 0 : defaultScrollSensitivity;

        if (Input.GetKey(KeyCode.LeftControl)) {
            zoom += (int)Input.mouseScrollDelta.y;

            zoom = Mathf.Min(maxZoom, Mathf.Max(zoom, 0));

            (transform as RectTransform).SetBottom(-zoom * 100);
            (transform as RectTransform).SetTop(-zoom * 100);
        }

        if (EditorManager.instance.beatsTotal != 0) {
            timeIndicator.localPosition = new Vector3(0, (EditorManager.instance.beat / (float)EditorManager.instance.beatsTotal) * -(transform as RectTransform).rect.height, 0);
        }
    }

    public void RedrawTimeline(AudioClip audioClip) {
        //texture = PaintWaveformSpectrum(audioClip, 10f, 512, 4096, Color.white);

        //image.GetComponent<RawImage>().texture = texture;

    }

    public IEnumerator RedrawTimelineCoroutine(AudioClip audioClip) {
        var width = 512;
        var height = 4096;

        texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
        image.GetComponent<RawImage>().texture = texture;

        yield return StartCoroutine(PaintWaveformSpectrumCoroutine(audioClip, 10f, width, height, Color.white, texture));
    }

    IEnumerator PaintWaveformSpectrumCoroutine (AudioClip audio, float saturation, int width, int height, Color col, Texture2D tex) {
        float[] samples = new float[audio.samples];
        float[] waveform = new float[height];
        audio.GetData(samples, 0);
        int packSize = (audio.samples / height) + 1;
        int s = 0;
        for (int i = 0; i < audio.samples; i += packSize) {
            waveform[s] = Mathf.Abs(samples[i]);
            s++;
        }

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                tex.SetPixel(x, y, Color.black);
            }
        }

        float time = Time.timeSinceLevelLoad;

        for (int y = 0; y < waveform.Length; y++) {
            for (int x = 0; x <= waveform[y] * ((float)width * .75f); x++) {
                tex.SetPixel(x, (height / 2) + y, col);
                tex.SetPixel(x, (height / 2) - y, col);
            }

            if (Time.timeSinceLevelLoad - time > 0.1f) {
                time = Time.timeSinceLevelLoad;

                yield return new WaitForEndOfFrame();
            }
        }

        tex.Apply();
    }

    public static Texture2D PaintWaveformSpectrum(AudioClip audio, float saturation, int width, int height, Color col) {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        float[] samples = new float[audio.samples];
        float[] waveform = new float[height];
        audio.GetData(samples, 0);
        int packSize = (audio.samples / height) + 1;
        int s = 0;
        for (int i = 0; i < audio.samples; i += packSize) {
            waveform[s] = Mathf.Abs(samples[i]);
            s++;
        }

        for (int x = 0; x < width; x++) {
            for (int y = 0; y < height; y++) {
                tex.SetPixel(x, y, Color.black);
            }
        }

        for (int y = 0; y < waveform.Length; y++) {
            for (int x = 0; x <= waveform[y] * ((float)width * .75f); x++) {
                tex.SetPixel(x, (height / 2) + y, col);
                tex.SetPixel(x, (height / 2) - y, col);
            }
        }

        tex.Apply();

        return tex;
    }

    public void OnPointerClick(PointerEventData eventData) {

        var rt = (transform as RectTransform);

        var normalizedY = 1 - (eventData.position.y - rt.offsetMin.y) / rt.rect.height;

        EditorManager.instance.SetBeat((int)Mathf.Round(normalizedY * (float)EditorManager.instance.beatsTotal));

        // OnClick code goes here ...
    }
}
