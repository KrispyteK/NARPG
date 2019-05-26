using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterSceneEditorInformation : Singleton<InterSceneEditorInformation> {

    public int beat;

    protected override bool DontDestroyOnLoad => true;

    protected override void OnInitiate() {
        gameObject.name = "InterSceneEditorInformation";

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        var m = FindObjectOfType<BeatManager>();

        if (m) {
            m.startBeat = beat;
        }

        Destroy(gameObject);
    }
}