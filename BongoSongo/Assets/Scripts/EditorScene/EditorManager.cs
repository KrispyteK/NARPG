using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour {
    public static EditorManager instance;

    public GameObject selected;
    public Level level;
    public LevelInfo levelInfo;

    void Awake() {
        instance = this;
    }

    void Start() {
        level = new Level {
            name = "test"
        };

        levelInfo.SetInfo();
    }

    void Update() {
        if (Input.GetKey(KeyCode.LeftControl)) {
            if (Input.GetKeyDown(KeyCode.S)) {
                Save();
            }
        }
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
