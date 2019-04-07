using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeatManager : MonoBehaviour {

    public static BeatManager instance;
    public static float beatLength;

    public SoundManager soundManager;
    public SpawnManager spawner;
    private bool audioStarted = false;
    public int beat = 0;
    public int i; // To count through the whenToSpawn array

    public float bpm;
    public bool spawnSoundOn;

    private int CurrentBeat => beat;

    void Awake() {
        beatLength = (60 / bpm);

        soundManager = FindObjectOfType<SoundManager>();
        spawner = FindObjectOfType<SpawnManager>();

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
            soundManager.beatTest.Play();
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
        if (i == spawner.spawnInfo.Count) return;

        while (beat == spawner.spawnInfo[i].beat) {
            // Use test sound to ensure spawning matches music track
            if (spawnSoundOn) {
                soundManager.hitBall.Play();
            }

            spawner.Spawn(spawner.spawnInfo[i]);

            i++;

            if (i == spawner.spawnInfo.Count) break;
        }

        if (i == spawner.spawnInfo.Count) {
            print("end of array");

            //i = 0;
            //beat = 0;
        }
    }
}
