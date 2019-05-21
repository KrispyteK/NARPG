using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimelineDrawer : MonoBehaviour {
    public RectTransform image;
    public ScrollRect scrollRect;
    public int maxZoom = 50;
    public float defaultScrollSensitivity;

    private Texture2D texture;
    private int zoom = 0;
    private Vector2 defaultSize;

    void Start() {
        RedrawTimeline();

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
    }

    public void RedrawTimeline() {
        var clip = EditorManager.instance.level.song.GenerateClip();

        texture = PaintWaveformSpectrum(clip, 10f, 512, 4096, Color.white);

        image.GetComponent<RawImage>().texture = texture;
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
}
