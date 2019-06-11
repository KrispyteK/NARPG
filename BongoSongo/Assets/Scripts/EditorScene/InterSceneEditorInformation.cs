using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterSceneEditorInformation : Singleton<InterSceneEditorInformation> {


    public int beat;
    public bool playAtBeat;

    protected override bool DontDestroyLoad => true;

    void Start () {
        var otherInterEditors = FindObjectsOfType<InterSceneEditorInformation>();

        foreach (var interEditor in otherInterEditors) {
            if (interEditor != this) {
                DestroyImmediate(gameObject);
                return;
            }
        }

        _instance = this;

        DontDestroyOnLoad(gameObject);

        OnInitiate();
    }

    protected override void OnInitiate() {
        gameObject.name = "InterSceneEditorInformation";

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        print("Loaded scene: " + scene);

        if (scene == SceneManager.GetSceneByName("GamePlayScene")) {
            if (playAtBeat) {
                var m = FindObjectOfType<BeatManager>();

                if (m) {
                    m.startBeat = beat;
                    m.doCountdown = false;
                    m.doOutline = false;
                }
            }
        } else if (scene == SceneManager.GetSceneByName("EditorScene")) {
            var m = FindObjectOfType<EditorManager>();

            print("Entered editor scene");

            m.LoadLevel(InterScene.Instance.level);
            m.SetBeat(beat);

            var windows = FindObjectsOfType<Window>();

            foreach (var window in windows) {
                window.panel.SetActive(false);
            }
        }
    }
}