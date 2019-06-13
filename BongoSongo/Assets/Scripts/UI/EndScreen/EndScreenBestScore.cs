using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenBestScore : MonoBehaviour {

    public GameObject newHighScoreEffects;
    public AudioSource newHighScoreAudio;

    void Start() {
        if (InterScene.Instance.level == null) return;

        var bestScore = HighScores.SaveNewHighScore(InterScene.Instance.level, InterScene.Instance.score);

        if (InterScene.Instance.score == bestScore) {
            newHighScoreAudio.Play();

            var particlePos = transform.position;
            particlePos.z = -2;

            var particles = Instantiate(newHighScoreEffects, particlePos, Quaternion.identity);
        }

        GetComponent<UnityEngine.UI.Text>().text = "best score: " + bestScore;
    }
}