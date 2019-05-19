using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour {
    public static EditorManager instance;

    public GameObject selected;
    public Level level;

    void Awake() {
        instance = this;
    }

    void Start() {
        level = new Level {
            name = "test"
        };
    }

    void Update() {
        if (Input.GetKey(KeyCode.LeftControl)) {
            if (Input.GetKeyDown(KeyCode.S)) {
                Save();
            }
        }
    }

    public static void Save () {
        Level.Save(instance.level);
    }
}
