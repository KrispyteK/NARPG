using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeatManager : MonoBehaviour {

    public static BeatManager instance;
    public static float beatLength;

    public SoundManager theSoundManager;
    public SpawnManager theSpawner;
    private bool audioStarted = false;
    public int bar = 0;
    public int beat = 1;
    public int i; // To count through the whenToSpawn array

    public float bpm;
    public bool spawnSoundOn;

    private int CurrentBeat => bar * 4 + beat;

    void Awake() {
        beatLength = (60 / bpm);

        theSoundManager = FindObjectOfType<SoundManager>();
        theSpawner = FindObjectOfType<SpawnManager>();

        if (instance == null) {
            instance = this;
        }
        else {
            Debug.LogError("Too many beat managers in the scene!");
        }
    }

    void Start() {
        InvokeRepeating("BeatEvent", beatLength, beatLength);
    }

    void BeatEvent() {
        if (!audioStarted) {
            theSoundManager.beatTest.Play();
            audioStarted = true;
        }

        CheckSpawners();
        BeatCount();
    }

    // Keep track of current bar and beat (in 4/4 time)
    public void BeatCount() {
        beat++;

        if (beat == 5) {
            bar++;
            beat = 1;
        }
    }

    public void CheckSpawners() {
        while (beat == theSpawner.spawnInfo[i].beat && bar == theSpawner.spawnInfo[i].bar) {
            // Use test sound to ensure spawning matches music track
            if (spawnSoundOn) {
                theSoundManager.hitBall.Play();
            }

            theSpawner.Spawn(theSpawner.spawnInfo[i]);

            i++;

            if (i == theSpawner.spawnInfo.Count) break;
        }

        if (i == theSpawner.spawnInfo.Count) {
            print("end of array");

            i = 0;
            bar = 0;
            beat = 1;
        }
    }
}
