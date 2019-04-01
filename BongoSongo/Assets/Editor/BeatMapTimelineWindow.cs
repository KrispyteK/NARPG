using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class BeatMapTimelineWindow : EditorWindowInput {
    private static SoundManager soundManager;
    private static BeatManager beatManager;
    private static Texture2D texture;

    public static int currentBeat;
    public static int zoom;
    public static float offset;

    public static int beats;

    [MenuItem("Mapping/Timeline Window")]
    public static void CreateMenu() {
        GetWindow<BeatMapTimelineWindow>();

        GetManagers();
    }

    private static void GetManagers() {
        soundManager = FindObjectOfType<SoundManager>();
        beatManager = FindObjectOfType<BeatManager>();

        texture = PaintWaveformSpectrum(soundManager.beatTest.clip, 10f, 16000, 512, Color.white);

        var floatBeats = soundManager.beatTest.clip.length / (beatManager.bpm/60);

        beats = (int)floatBeats;
    }

    private void OnGUI() {
        if (soundManager == null) GetManagers();

        GUI.DrawTexture(new Rect(0, 25, Screen.width * (zoom + 1), Screen.height - 25), texture);

        CheckInput();

        CheckSideScroll();

        DrawQuad(new Rect(Screen.width * currentBeat / beats, 25, 2, Screen.height-25), Color.red);

        GUI.Label(new Rect(5, 5, 50, 50),"Beat: " + currentBeat);
    }

    public void CheckSideScroll () {
        if (OnPressed(KeyCode.RightArrow)) {
            currentBeat += 1;

            if (currentBeat > beats - 1) currentBeat = 0;
        }
        else if (OnPressed(KeyCode.LeftArrow)) {
            currentBeat -= 1;

            if (currentBeat < 0) currentBeat = beats - 1;
        }
    }

    protected override void OnScroll(int delta) {
        //if (isHoldingCtrl) {
        //    if (zoom > 0) {
        //        offset += delta / zoom;
        //    }
        //}
        //else {
        //    zoom += delta;

        //    if (zoom < 0) zoom = 0;
        //}
    }

    public static Texture2D PaintWaveformSpectrum(AudioClip audio, float saturation, int width, int height, Color col) {
        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        float[] samples = new float[audio.samples];
        float[] waveform = new float[width];
        audio.GetData(samples, 0);
        int packSize = (audio.samples / width) + 1;
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

        for (int x = 0; x < waveform.Length; x++) {
            for (int y = 0; y <= waveform[x] * ((float)height * .75f); y++) {
                tex.SetPixel(x, (height / 2) + y, col);
                tex.SetPixel(x, (height / 2) - y, col);
            }
        }
        tex.Apply();

        return tex;
    }
}
