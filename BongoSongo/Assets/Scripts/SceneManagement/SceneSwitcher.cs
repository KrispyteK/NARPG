using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour {
    public static void LoadScene(string scene) {
        SceneManager.LoadScene("LoadingScreen");

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);
    }
}
