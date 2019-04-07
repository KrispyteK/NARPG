using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterScene : MonoBehaviour {
    public static InterScene instance;

    public int score;

    void Awake () {
        if (instance == null) {
            instance = this;
        }
        else {
            Debug.LogError("Too many game managers in the scene!");
        }

        DontDestroyOnLoad(gameObject);
    }
}
