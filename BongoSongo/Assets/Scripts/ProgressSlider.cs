using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressSlider : MonoBehaviour {

    private AudioSource song;
    private Slider slider;

    void Start() {
        var soundManager = FindObjectOfType<SoundManager>();
        song = soundManager.beatTest;

        slider = GetComponent<Slider>();
    }

    void Update() {
        slider.value = song.time / song.clip.length;
    }
}
