using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class BeatMapTimelineWindow : EditorWindow {
    private static SoundManager soundManager;
    private static Texture2D texture;

    [MenuItem("Mapping/Timeline Window")]
    public static void CreateMenu() {
        GetWindow<BeatMapTimelineWindow>();

        GetManagers();
    }

    private static void GetManagers() {
        soundManager = FindObjectOfType<SoundManager>();

        texture = PaintWaveformSpectrum(soundManager.beatTest.clip, 10f, 4096, 4096, Color.white);
    }

    private void OnGUI () {
        if (soundManager == null) GetManagers();
        GUI.DrawTexture(new Rect(0,0, Screen.width, Screen.height), texture);
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
