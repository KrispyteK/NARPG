using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndScreenStars : MonoBehaviour {

    public Sprite filled;
    public Image[] stars;

    private int GetTotalPossibleScore () {
        var level = InterScene.Instance.level;

        if (level == null) return 1;

        var settings = InterScene.Instance.gamePlaySettings;
        var score = 0;

        foreach (var spawnInfo in level.spawnInfo) {
            switch (spawnInfo.indicator) {
                case Indicators.Button:
                    score += settings.indicatorScoreOnTime;
                    break;
                case Indicators.Slider:
                    score += spawnInfo.beatLength * settings.sliderScore;
                    break;
                default:
                    break;
            }
        }

        return score;
    }

    void Start() {
        var totalScorePossible = GetTotalPossibleScore();

        float ratio = InterScene.Instance.score / (float)totalScorePossible;

        for (int i = 0; i < stars.Length; i++) {
            var image = stars[i];
            var normalized = i / ((float)stars.Length + 1);

            if (ratio > normalized) {
                image.sprite = filled;
            }
        }
    }
}
