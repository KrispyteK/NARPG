using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenBestScore : MonoBehaviour {
    void Start() {
        if (InterScene.Instance.level == null) return;

        var bestScore = HighScores.SaveNewHighScore(InterScene.Instance.level, InterScene.Instance.score);

        GetComponent<UnityEngine.UI.Text>().text = "best score: " + bestScore;
    }
}