using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelInfo : MonoBehaviour {
    public TMP_InputField inputName;
    public TMP_InputField inputBPM;
    public TMP_InputField inputDescription;

    public void SetInfo () {
        if (!string.IsNullOrEmpty(EditorManager.instance.level.name)) inputName.text = EditorManager.instance.level.name;
        if (!string.IsNullOrEmpty(EditorManager.instance.level.description)) inputDescription.text = EditorManager.instance.level.description;

        inputBPM.text = "" + EditorManager.instance.level.bpm;
    }
}
