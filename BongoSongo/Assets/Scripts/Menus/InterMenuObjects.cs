using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class InterMenuObjects : MonoBehaviour {

    public string[] scenes;

    void Start() {
        var otherInterEditors = FindObjectsOfType<InterMenuObjects>();

        foreach (var interEditor in otherInterEditors) {
            if (interEditor != this) {
                DestroyImmediate(gameObject);
                return;
            }
        }

        SceneManager.sceneLoaded += OnSceneLoaded;

        DontDestroyOnLoad(gameObject);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scenes.Contains(scene.name)) {
            gameObject.SetActive(true);
        } else {
            gameObject.SetActive(false);
        }
    }
}
