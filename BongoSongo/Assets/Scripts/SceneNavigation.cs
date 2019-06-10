using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneNavigation : MonoBehaviour {
    public void StartDance() {
        SceneManager.LoadScene("DanceScene1");
    }

    public void StartTechno() {
        SceneManager.LoadScene("TechnoScene");
    }

    public void SelectSong() {
        SceneManager.LoadScene("SongSelect");
    }

    public void HomeScreen() {
        SceneManager.LoadScene("HomeScreen");
    }

    public void EndScreen() {
        SceneManager.LoadScene("EndScreen");
    }

    public void CloseApp() {
        Application.Quit();
        Debug.Log("Quiting");
    }

    public void OpenScene (string scene) {
        SceneManager.LoadScene(scene);
    }

    public void Restart () {
        //var interScene = InterScene.Instance;
        //var level = Level.Load(scrollRectSnap.Selected.GetComponent<SongOption>().level + ".json");

        //interScene.level = level;

        SceneManager.LoadScene("GameplayScene");
    }

    void Update () {
        if (Input.GetButtonDown("Cancel")) {
            var scene = SceneManager.GetActiveScene();

            switch (scene.name) {
                case "HomeScreen":
                    Application.Quit();
                    Debug.Log("Quiting");
                    break;
                case "EndScreen":
                case "SongSelect":
                    HomeScreen();
                    break;
            }
        }
    }
}
