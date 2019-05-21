using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum Indicators {
    Button
}

[System.Serializable]
public struct EditorPrefab {
    public Indicators indicator;
    public GameObject prefab;
    public Sprite preview;
}

public class EditorManager : MonoBehaviour {
    public static EditorManager instance;

    public EditorPrefab currentPrefab;
    public GameObject selected;
    public Level level;
    public LevelInfo levelInfo;
    public Transform indicatorParent;
    public int beat;
    public Text beatText;

    public List<EditorPrefab> editorPrefabs = new List<EditorPrefab>();

    public float beatLength;
    public int beatsTotal;

    private TimelineDrawer timelineDrawer;

    void Awake() {
        instance = this;
    }

    void Start() {
        level = new Level {
            name = "test"
        };

        levelInfo.SetInfo();

        currentPrefab = editorPrefabs[0];

        timelineDrawer = FindObjectOfType<TimelineDrawer>();
    }

    void Update() {
        if (Input.GetKey(KeyCode.LeftControl)) {
            if (Input.GetKeyDown(KeyCode.S)) {
                Save();
            }
        }
    }

    public void LoadSong () {
        var clip = level.song.GenerateClip();

        timelineDrawer.RedrawTimeline(clip);

        beatLength = 60f / level.bpm;

        beatsTotal = (int)Mathf.Floor(clip.length / beatLength);
    }

    public void IncreaseBeat () {
        SetBeat(beat + 1);

        if (beat >= beatsTotal - 1) SetBeat(0);
    }

    public void DecreaseBeat() {
        SetBeat(beat - 1);

        if (beat <= 0) SetBeat(beatsTotal - 1);
    }


    public void SetBeat (int num) {
        beat = num;

        beatText.text = "Beat: " + num;
    }

    public void CreateNewIndicator () {
        Instantiate(currentPrefab.prefab, Vector3.zero, Quaternion.identity, indicatorParent);
    }

    public void SetName(TMPro.TMP_InputField inputField) {
        level.name = inputField.text;
    }

    public void SetBPM(TMPro.TMP_InputField inputField) {
        level.bpm = int.Parse(inputField.text);

        LoadSong();
    }

    public void Save () {
        Level.Save(level);
    }

    public void New() {
        Level.Save(level);

        level = new Level();

        levelInfo.SetInfo();
    }
}
