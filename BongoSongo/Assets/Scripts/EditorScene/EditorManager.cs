using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorManager : MonoBehaviour {

    public Level level;

    void Start() {
        level = new Level {
            name = "test"
        };
    }

    void Update() {
        if (Input.GetKey(KeyCode.LeftControl)) {
            if (Input.GetKeyDown(KeyCode.S)) {
                Level.Save(level);
            }
        }
    }
}
