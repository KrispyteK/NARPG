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
    public int beat = 0;
    public int i; // To count through the whenToSpawn array

    public float bpm;
    public bool spawnSoundOn;

    private int CurrentBeat => beat;

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
        //InvokeRepeating("BeatEvent", beatLength, beatLength);

        StartCoroutine(Countdown());
    }

    void BeatEvent() {
        if (!audioStarted) {
            theSoundManager.beatTest.Play();
            audioStarted = true;
        }

        CheckSpawners();
        BeatCount();
    }

    private IEnumerator Countdown () {
        var count = 3;

        while (count > 0) {
            FloatingText.Create(Vector2.zero, "" + count, 1);

            yield return new WaitForSeconds(1f);

            count--;
        }

        InvokeRepeating("BeatEvent", beatLength, beatLength);
    }

    public void BeatCount() {
        beat++;
    }

    public void CheckSpawners() {
        if (i == theSpawner.spawnInfo.Count) return;

        while (beat == theSpawner.spawnInfo[i].beat) {
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

            //i = 0;
            //beat = 0;
        }
    }
}
