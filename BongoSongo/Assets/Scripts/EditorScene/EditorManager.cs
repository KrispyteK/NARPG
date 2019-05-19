using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public List<EditorPrefab> editorPrefabs = new List<EditorPrefab>(); 

    void Awake() {
        instance = this;
    }

    void Start() {
        level = new Level {
            name = "test"
        };

        levelInfo.SetInfo();

        currentPrefab = editorPrefabs[0];
    }

    void Update() {
        if (Input.GetKey(KeyCode.LeftControl)) {
            if (Input.GetKeyDown(KeyCode.S)) {
                Save();
            }
        }
    }

    public void CreateNewIndicator () {
        Instantiate(currentPrefab.prefab, Vector3.zero, Quaternion.identity);
    }

    public void SetName(TMPro.TMP_InputField inputField) {
        level.name = inputField.text;
    }

    public void SetBPM(TMPro.TMP_InputField inputField) {
        level.bpm = int.Parse(inputField.text);
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
