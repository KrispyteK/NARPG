using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class BeatMapWindow : EditorWindowInput {
    private int timeLineHeight = 100;

    private SoundManager soundManager;
    private SpawnManager spawnManager;
    private GameManager gameManager;
    private BeatManager beatManager;
    private Texture2D texture;

    private int modifyingIndex;
    private bool isModifying;
    private Vector2 offset;
    private int currentBeat;
    private int beats;
    private float beatSecond;
    private float songLength;
    private float beatSongLength;

    private float Size => Screen.width * gameManager.buttonSize;


    [MenuItem("Mapping/Window")]
    public static void CreateMenu() {
        var window = GetWindow<BeatMapWindow>();

        window.GetManagers();
    }

    private void GetManagers() {
        spawnManager = FindObjectOfType<SpawnManager>();
        gameManager = FindObjectOfType<GameManager>();
        soundManager = FindObjectOfType<SoundManager>();
        beatManager = FindObjectOfType<BeatManager>();

        texture = PaintWaveformSpectrum(soundManager.beatTest.clip, 4096, 512, Color.white);

        var floatBeats = soundManager.beatTest.clip.length / (beatManager.bpm / 60);

        beats = (int)floatBeats;

        beatSecond = (beatManager.bpm / 60);

        songLength = soundManager.beatTest.clip.length;

        beatSongLength = beats * beatSecond;

        Debug.Log($"{songLength} {beatSongLength}");
    }

    void Update() {
        if (focusedWindow == this) Repaint();
    }

    public void OnGUI() {
        if (spawnManager == null) GetManagers();

        GUI.Label(new Rect(5, 5, 50, 50), "Beat: " + currentBeat);

        var e = Event.current;
        var controlID = GUIUtility.GetControlID(FocusType.Passive);

        switch (e.GetTypeForControl(controlID)) {
            case EventType.MouseUp:
                isModifying = false;

                e.Use();
                break;

            case EventType.MouseDown:
                CheckClick();

                e.Use();
                break;

            case EventType.KeyDown:
                switch (e.keyCode) {
                    case KeyCode.RightArrow:
                        currentBeat++;

                        if (currentBeat > beats - 1) currentBeat = 0;
                        break;
                    case KeyCode.LeftArrow:
                        currentBeat--;

                        if (currentBeat < 0) currentBeat = beats - 1;
                        break;
                }

                break;
        }

        DisplayBeats();
        ModifyButton();

        GUI.DrawTexture(new Rect(0, Screen.height - timeLineHeight, Screen.width, timeLineHeight), texture);

        var actualLength = beats * beatSecond;

        DrawQuad(new Rect(((currentBeat * beatSecond) / actualLength) * Screen.width, Screen.height - timeLineHeight, 1, timeLineHeight), Color.red);
    }

    private void ModifyButton() {
        if (!isModifying) return;

        var position = (Event.current.mousePosition + new Vector2(Size, Size) / 2 + offset) / new Vector2(Screen.width, Screen.height);

        spawnManager.spawnInfo[modifyingIndex].position = new Vector2(Mathf.Clamp01(position.x), Mathf.Clamp01(position.y));
    }

    private void CheckClick() {
        var mousePos = Event.current.mousePosition;

        for (int i = 0; i < spawnManager.spawnInfo.Count; i++) {
            var spawnInfo = spawnManager.spawnInfo[i];
            var position = new Vector2(
                    spawnInfo.position.x * Screen.width - Size / 2,
                    spawnInfo.position.y * Screen.height - Size / 2
                );

            var rect = new Rect(position, new Vector2(Size, Size));

            if (rect.Contains(mousePos)) {
                isModifying = true;
                modifyingIndex = i;
                offset = position - mousePos;

                return;
            }
        }
    }

    private void DisplayBeats() {
        var beat = currentBeat;

        var start = Mathf.Max(0, beat - 3);
        var end = Mathf.Min(beats, beat + 3);

        for (int i = start; i < spawnManager.spawnInfo.Count; i++) {
            var spawnInfo = spawnManager.spawnInfo[i];
            var spawnInfoBeat = spawnInfo.bar * 4 + spawnInfo.beat;

            if (spawnInfoBeat < start || spawnInfoBeat > end) continue; 

            var position = new Vector2(
                    spawnInfo.position.x * Screen.width - Size / 2,
                    spawnInfo.position.y * Screen.height - Size / 2
                );

            DrawQuad(new Rect(position, new Vector2(Size, Size)), new Color(255,255,255, 1f - ((beat - spawnInfoBeat) / 4f)));

            GUI.Label(new Rect(position, new Vector2(Size, Size)), "" + spawnInfoBeat);
        }
    }

    public static AudioClip CloneAudioClip(AudioClip audioClip) {
        AudioClip newAudioClip = AudioClip.Create("", audioClip.samples, audioClip.channels, audioClip.frequency, false);
        float[] copyData = new float[audioClip.samples * audioClip.channels];
        audioClip.GetData(copyData, 0);

        List<float> monoData = new List<float>();
        for (int i = 0; i < copyData.Length; i += 2) {
            monoData.Add(copyData[i]);
        }
        newAudioClip.SetData(monoData.ToArray(), 0);

        return newAudioClip;
    }


    public static Texture2D PaintWaveformSpectrum(AudioClip audio, int width, int height, Color col) {

        audio = CloneAudioClip(audio);

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
