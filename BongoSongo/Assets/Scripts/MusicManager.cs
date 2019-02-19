using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {


    public Song Song;
    public float Smoothing;
    public float CombinedBass;
    public float MeasuredBass;
    public float TimedBass;

    public float beatOffset;
    public int spectrumBassMin = 0;
    public int spectrumBassMax = 10;
    public float spectrumBassThreshold = 0.01f;

    public static MusicManager Instance;

    private AudioSource audioSource;
    private float[] samples = new float[1024];
    private Action action;

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
        audioSource.time = 0;

        StartCoroutine(BPMCycle());
    }

    public void OnBeat (Action a) {
        action += a;
    }

    void FixedUpdate() {
        audioSource.GetSpectrumData(samples, 0, FFTWindow.Blackman);

        MeasuredBass = 0;
        int count = 0;

        for (int i = spectrumBassMin; i < spectrumBassMax; i++) {
            var value = Mathf.Max(samples[i] / audioSource.volume - spectrumBassThreshold, 0);

            if (value > 0) {
                //Debug.DrawLine(new Vector3(i * 0.001f, 0), new Vector3(i * 0.001f, samples[i]), Color.cyan);

                MeasuredBass += value;

                count++;
            }
        }

        if (count > 0) MeasuredBass /= count;

        MeasuredBass = Mathf.Pow(MeasuredBass, 0.25f);

        var desiredBass = (MeasuredBass + TimedBass) / 2f;

        if (desiredBass > CombinedBass) {
            CombinedBass = desiredBass;
        } else {
            CombinedBass -= CombinedBass * Time.fixedDeltaTime * Smoothing;
        }

        TimedBass -= CombinedBass * Time.fixedDeltaTime * Smoothing;

        //Debug.DrawLine(new Vector3(-0.005f, 0), new Vector3(-0.005f, Bass), Color.red);
    }

    IEnumerator BPMCycle () {
        var time = 1f / (Song.BPM / 60f);

        yield return new WaitForSeconds(time * beatOffset);

        while (true) {
            yield return new WaitForSeconds(time);

            TimedBass = 1f;

            action();
        }
    }
}
