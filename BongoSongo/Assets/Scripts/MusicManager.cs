using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour {


    public Song Song;
    public float Bass;
    public float spectrumBass;
    public float spectrumBassMin = 0;
    public float spectrumBassMax = 10;

    public static MusicManager Instance;

    private AudioSource audioSource;
    private float[] samples = new float[1024];


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

        spectrumBass = 0;

        for (int i = 0; i < samples.Length; i++) {
            Debug.DrawLine(new Vector3(i * 0.001f, 0), new Vector3(i * 0.001f, samples[i]), Color.cyan);

            if (i > spectrumBassMin && i < spectrumBassMax) {
                spectrumBass += samples[i];
            }
        }

        spectrumBass /= (spectrumBassMax - spectrumBassMin);

        float desiredBass = spectrumBass;

        if (desiredBass > Bass) {
            Bass = desiredBass;
        } else {
            Bass -= Bass * Time.deltaTime * 5f;
        }

        Debug.DrawLine(new Vector3(-0.005f, 0), new Vector3(-0.005f, Bass), Color.red);
    }
}
