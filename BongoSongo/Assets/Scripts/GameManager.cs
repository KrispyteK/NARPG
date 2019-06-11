using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    public int score;
    public float buttonSize;

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

        InterScene.Instance.score = score;
    }

    void Update() {
        if (Input.GetButtonDown("Cancel")) {
            var interEditor = FindObjectOfType<InterSceneEditorInformation>();

            if (interEditor) {
                SceneManager.LoadScene("EditorScene");
            }
            else {
                End();
            }
        }
    }

    public void End() {
        SceneManager.LoadScene("EndScreen");

        WebcamMotionCapture.instance.webcamTexture.Stop();
    }
}
