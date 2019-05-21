using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;

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
    public Transform indicatorParent;
    public int beat;
    public Text beatText;
    public LevelInfo levelInfo;

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

        currentPrefab = editorPrefabs[0];

        timelineDrawer = FindObjectOfType<TimelineDrawer>();
        levelInfo = FindObjectOfType<LevelInfo>();
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

        beatText.text = "Beat: " + beat;
    }

    public void OrderIndicators () {
        var infos = FindObjectsOfType<IndicatorInfo>();
        var ordered = infos.OrderBy(x => x.beat).ToArray();

        for (int i = 0; i < ordered.Length; i++) {
            ordered[i].transform.SetSiblingIndex(i);
        }
    }

    public List<SpawnInfo> GenerateSpawnInfoList () {
        var infos = FindObjectsOfType<IndicatorInfo>();
        var ordered = infos.OrderBy(x => x.beat).ToArray();
        var list = new List<SpawnInfo>();

        for (int i = 0; i < ordered.Length; i++) {
            var spawnInfo = new SpawnInfo {
                beat = ordered[i].beat
            };

            var pos = ordered[i].transform.position / Camera.main.orthographicSize;

            spawnInfo.x = pos.x;
            spawnInfo.y = pos.y;

            list.Add(spawnInfo);
        }

        return list;
    }

    public void CreateNewIndicator () {
        var instance = Instantiate(currentPrefab.prefab, Vector3.zero, Quaternion.identity, indicatorParent);

        instance.GetComponent<IndicatorInfo>().beat = beat;

        OrderIndicators();
    }

    public void SetName(TMPro.TMP_InputField inputField) {
        level.name = inputField.text;
    }

    public void SetBPM(TMPro.TMP_InputField inputField) {
        level.bpm = int.Parse(inputField.text);

        LoadSong();
    }

    public void Save () {
        var spawnInfoList = GenerateSpawnInfoList();

        level.spawnInfo = spawnInfoList;

        Level.Save(level);
    }

    public void Load (string file) {
        foreach (Transform child in indicatorParent) {
            Destroy(child.gameObject);
        }

        level = Level.Load(file);

        levelInfo.SetInfo();
        LoadSong();

        foreach (var spawnInfo in level.spawnInfo) {
            var prefab = editorPrefabs.Find(x => x.indicator == spawnInfo.indicator).prefab;
            var position = new Vector2 (spawnInfo.x, spawnInfo.y) * Camera.main.orthographicSize;

            var instance = Instantiate(prefab, position, Quaternion.identity, indicatorParent);
            instance.GetComponent<IndicatorInfo>().beat = spawnInfo.beat;

            OrderIndicators();
        }
    }

    public void New() {
        Level.Save(level);

        level = new Level();

        levelInfo.SetInfo();
    }
}
