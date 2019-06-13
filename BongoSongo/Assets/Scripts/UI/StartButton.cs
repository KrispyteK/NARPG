using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartButton : MonoBehaviour {

    public ScrollRectSnap scrollRectSnap;
    public RectTransform editorPanel;

    public void Play() {
        if (scrollRectSnap.Selected == editorPanel) {
            SceneManager.LoadScene("EditorScene");
        } else { 
            var interScene = InterScene.Instance;
            var level = Level.LoadFromFullPath(scrollRectSnap.Selected.GetComponent<SongOption>().level);

            interScene.level = level;

            SceneManager.LoadScene("GameplayScene");
        }
    }
}
