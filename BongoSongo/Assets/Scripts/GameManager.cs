using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public int score;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Debug.LogError("Too many game managers in the scene!");
        }
    }

    public void AddScore(int add) {
        score += add;
    }

    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
