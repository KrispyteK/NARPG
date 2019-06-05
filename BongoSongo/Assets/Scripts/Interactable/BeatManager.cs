using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class BeatManager : MonoBehaviour {

    public static BeatManager instance;
    public static float beatLength;

    public SoundManager soundManager;
    public SpawnManager spawner;
    public GameObject outlineObject;
    public int startBeat;
    public bool doOutline = true;
    public float outlineTime = 5f;
    public bool doCountdown = true;
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
        StartAt(startBeat);
    }

    public void StartAt(int startBeat) {
        var newIndex = 0;

        for (int j = 0; j < spawner.spawnInfo.Count; j++) {
            if (spawner.spawnInfo[j].beat == startBeat) {
                newIndex = j;
            }
        }

        i = newIndex;

        beat = startBeat;

        if (doCountdown) {
            StartCoroutine(Countdown());
        }
        else {
            outlineObject.SetActive(false);

            InvokeRepeating("BeatEvent", beatLength, beatLength);
        }
    }

    void Update () {
        if (audioStarted && !soundManager.beatTest.isPlaying) {
            GameManager.instance.End();
        }
    }

    void BeatEvent() {
        if (!audioStarted) {
            var startTime = beatLength * beat;

            soundManager.beatTest.Play();
            soundManager.beatTest.time = startTime;

            audioStarted = true;
        }

        CheckSpawners();
        BeatCount();
    }

    private IEnumerator Countdown () {
        var canvas = GameObject.Find("UI");
        canvas.SetActive(false);

        if (doOutline) yield return new WaitForSeconds(outlineTime);

        outlineObject.SetActive(false);

        var count = 3;

        while (count > 0) {
            FloatingText.Create(Vector2.zero, "" + count, 1);

            yield return new WaitForSeconds(1f);

            count--;
        }

        canvas.SetActive(true);

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
