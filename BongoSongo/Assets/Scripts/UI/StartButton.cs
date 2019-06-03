using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour {

    public ScrollRectSnap scrollRectSnap;

    public void Play() {
        var interScene = InterScene.Instance;
        var level = Level.Load(scrollRectSnap.Selected.GetComponent<SongOption>().level + ".level");

        interScene.level = level;

        SceneManager.LoadScene("GameplayScene");
    }
}
