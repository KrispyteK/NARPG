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
