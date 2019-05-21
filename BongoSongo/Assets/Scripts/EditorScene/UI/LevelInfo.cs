using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelInfo : MonoBehaviour {
    public TMP_InputField inputName;
    public TMP_InputField inputBPM;
    public Text songText;

    public void SetInfo () {
        if (!string.IsNullOrEmpty(EditorManager.instance.level.name)) inputName.text = EditorManager.instance.level.name;

        print(EditorManager.instance.level.song.GenerateClip());

        songText.text = EditorManager.instance.level.song.GenerateClip().name;

        inputBPM.text = "" + EditorManager.instance.level.bpm;
    }
}
