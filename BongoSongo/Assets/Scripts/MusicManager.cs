using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {


    public Song Song;
    public float Bass;

    public static MusicManager Instance;

    private AudioSource audioSource;
    private float[] samples = new float[256];

    void Awake () {
        if (Instance == null) {
            Instance = this;
        } else {
            Debug.LogError("A MusicManager already exists in the scene!");
        }
    }

    void Start() {
        audioSource = GetComponent<AudioSource>();

        audioSource.clip = Song.Clip;
        audioSource.Play();
        audioSource.time = 72;
    }

    void Update() {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);

        for (int i = 0; i < samples.Length; i++) {
            Debug.DrawLine(new Vector3(i * 0.001f, 0), new Vector3(i * 0.001f, samples[i]), Color.cyan);
        }
    }
}
