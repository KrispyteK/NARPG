using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad]
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
    private int selected = -1;
    private static int defaultStartBeat;
    private bool setDefaultStartBeat;

    private float Size => Screen.width * gameManager.buttonSize;

    // register an event handler when the class is initialized
    //static BeatMapWindow() {
    //    EditorApplication.playModeStateChanged += LogPlayModeState;
    //}

    //private static void LogPlayModeState(PlayModeStateChange state) {
    //    if (state == PlayModeStateChange.EnteredEditMode) {
    //        FindObjectOfType<BeatManager>().startBeat = defaultStartBeat;
    //    }
    //}

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

        beatSecond = 60 / beatManager.bpm;

        var floatBeats = soundManager.beatTest.clip.length / beatSecond;

        beats = (int)floatBeats;

        songLength = soundManager.beatTest.clip.length;

        beatSongLength = beats * beatSecond;

        texture = PaintWaveformSpectrum(soundManager.beatTest.clip, 4096, 512, Color.white);

        SortList();
    }

    void Update() {
        if (focusedWindow == this) Repaint();
    }

    private void Input() {
        var e = Event.current;
        var controlID = GUIUtility.GetControlID(FocusType.Passive);

        switch (e.GetTypeForControl(controlID)) {
            case EventType.MouseUp:
                isModifying = false;

                e.Use();
                break;

            case EventType.MouseDown:
                var isClickingTimeLine = e.mousePosition.y > Screen.height - timeLineHeight;

                if (isClickingTimeLine) {
                    ClickTimeLine();
                } else {
                    CheckClick();
                }

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

                    case KeyCode.Delete:
                        if (selected > -1) {
                            spawnManager.spawnInfo.RemoveAt(selected);

                            selected = -1;

                            SortList();

                            Undo.RecordObject(spawnManager, "Delete Beat");
                        }


                        break;

                    case KeyCode.Space:

                        int beat = currentBeat;
                        var position = (Event.current.mousePosition) / new Vector2(Screen.width, Screen.height);

                        spawnManager.spawnInfo.Insert(0, new SpawnInfo {
                            beat = beat,
                            position = position
                        });

                        selected = -1;

                        SortList();

                        Undo.RecordObject(spawnManager, "Add Beat");

                        break;

                    case KeyCode.Z:
                        //defaultStartBeat = beatManager.startBeat;

                        beatManager.startBeat = currentBeat;

                        EditorApplication.isPlaying = true;

                        //setDefaultStartBeat = true;

                        break;
                }

                break;
        }
    }

    public void OnGUI() {
        if (spawnManager == null) GetManagers();

        GUI.Label(new Rect(5, 5, 250, 50), "Beat: " + currentBeat);

        Input();

        DisplayBeats();
        ModifyButton();

        GUI.DrawTexture(new Rect(0, Screen.height - timeLineHeight, Screen.width, timeLineHeight), texture);

        DrawQuad(new Rect(((currentBeat * beatSecond) / beatSongLength) * Screen.width, Screen.height - timeLineHeight, 1, timeLineHeight), Color.red);
    }

    private void SortList() {
        spawnManager.spawnInfo.Sort((x, y) => {
            return x.beat - y.beat;
        });
    }

    private void ModifyButton() {
        if (!isModifying) return;

        var position = (Event.current.mousePosition + new Vector2(Size, Size) / 2 + offset) / new Vector2(Screen.width, Screen.height);

        spawnManager.spawnInfo[modifyingIndex].position = new Vector2(Mathf.Clamp01(position.x), Mathf.Clamp01(position.y));

        Undo.RecordObject(spawnManager, "Move Beat");
    }

    private void ClickTimeLine() {
        var beat = (int)(Event.current.mousePosition.x / Screen.width * beats + 0.5f);

        currentBeat = beat;
    }

    private void CheckClick() {
        var mousePos = Event.current.mousePosition;

        var start = Mathf.Max(0, currentBeat - 5);
        var end = Mathf.Min(beats, currentBeat + 5);

        for (int i = 0; i < spawnManager.spawnInfo.Count; i++) {
            var spawnInfo = spawnManager.spawnInfo[i];
            var spawnInfoBeat = spawnInfo.beat;

            if (spawnInfoBeat < start || spawnInfoBeat > end) continue;

            var position = new Vector2(
                    spawnInfo.position.x * Screen.width - Size / 2,
                    spawnInfo.position.y * Screen.height - Size / 2
                );

            var rect = new Rect(position, new Vector2(Size, Size));

            if (rect.Contains(mousePos)) {
                isModifying = true;
                modifyingIndex = i;
                selected = i;
                offset = position - mousePos;

                return;
            }
        }

        selected = -1;
    }

    private void DisplayBeats() {
        var beat = currentBeat;

        var start = Mathf.Max(0, beat - 5);
        var end = Mathf.Min(beats, beat + 5);

        var beatDictionary = new Dictionary<int, int>();

        for (int i = 0; i < spawnManager.spawnInfo.Count; i++) {
            var spawnInfo = spawnManager.spawnInfo[i];
            var spawnInfoBeat = spawnInfo.beat;

            if (beatDictionary.ContainsKey(spawnInfoBeat)) {
                beatDictionary[spawnInfoBeat]++;
            }
            else {
                beatDictionary[spawnInfoBeat] = 1;
            }

            if (spawnInfoBeat < start || spawnInfoBeat > end) continue;

            var position = new Vector2(
                    spawnInfo.position.x * Screen.width - Size / 2,
                    spawnInfo.position.y * Screen.height - Size / 2
                );

            DrawQuad(new Rect(position, new Vector2(Size, Size)), new Color((i == selected ? 0 : 255), 255, 255, 1f - (Mathf.Abs(beat - spawnInfoBeat) / 6f)));

            GUI.Label(new Rect(position, new Vector2(Size, Size)), "" + spawnInfoBeat);
        }

        var displayedBeat = 0; 

        foreach (var kv in beatDictionary) {
            var beatPos = kv.Key;
            var amount = kv.Value;

            for (int i = 0; i < amount; i++) {
                DrawQuad(
                    new Rect(((beatPos * beatSecond) / beatSongLength) * Screen.width, Screen.height - timeLineHeight - 10 - i * 8, 3, 3),
                    new Color((displayedBeat == selected ? 0 : 255), 255, 255)
                    );

                displayedBeat++;
            }
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


    public Texture2D PaintWaveformSpectrum(AudioClip audio, int width, int height, Color col) {
        audio = CloneAudioClip(audio);

        Texture2D tex = new Texture2D(width, height, TextureFormat.RGBA32, false);
        float[] samples = new float[audio.samples];
        float[] waveform = new float[width];
        int packSize = (audio.samples / width) + 1;

        audio.GetData(samples, 0);

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
