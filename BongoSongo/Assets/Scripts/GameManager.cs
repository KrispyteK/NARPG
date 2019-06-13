using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public int score;
    public int combo = 1;
    public float buttonSize;

    public Text comboText;

    private int indicatorHits;
    private SoundManager soundManager;

    void Awake() {
        if (instance == null) {
            instance = this;
        }
        else {
            Debug.LogError("Too many game managers in the scene!");
        }

        soundManager = FindObjectOfType<SoundManager>();
    }

    public int AddScore(int add) {
        var added = add * combo;

        score += add * combo;

        InterScene.Instance.score = score;

        return added;
    }

    public void OnIndicatorHit () {
        indicatorHits++;

        if (indicatorHits == InterScene.Instance.gamePlaySettings.indicatorHitsNeededForCombo) {
            indicatorHits = 0;
            AddCombo();
        }
    }

    public void AddCombo () {
        combo++;

        comboText.text = combo + "x";

        soundManager.comboGain.Play();
    }

    public void RemoveCombo() {
        indicatorHits = 0;

        combo--;

        combo = Mathf.Max(1, combo);

        comboText.text = combo + "x";

        soundManager.comboFail.Play();
    }


    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            var interEditor = FindObjectOfType<InterSceneEditorInformation>();

            if (interEditor) {
                End("EditorScene");
            }
            else {
                End("EndScreen");
            }
        }
    }

    public void End(string scene) {
        SceneManager.LoadScene(scene);

        WebcamMotionCapture.instance.webcamTexture.Stop();
    }
}
