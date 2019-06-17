using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSelectContent : MonoBehaviour {

    public GameObject editorPanel;
    public GameObject songPanel;
    public GameObject scrollPanel;

    private ScrollRectSnap scrollRectSnap;

    void Start() {
        scrollRectSnap = GetComponent<ScrollRectSnap>();

        GenerateButtons();
    }

    public void GenerateButtons() {
        foreach (Transform child in scrollPanel.transform) {
            Destroy(child.gameObject);
        }

        var files = new List<string>(Directory.GetFiles(DataManagement.StandardLevels, "*.json", SearchOption.AllDirectories));

        files.AddRange(Directory.GetFiles(DataManagement.Levels, "*.json", SearchOption.AllDirectories));

        var buttons = new List<RectTransform>();
        var i = 0;

        var panelEditor = Instantiate(editorPanel, scrollPanel.transform);
        var rtpe = panelEditor.GetComponent<RectTransform>();
        rtpe.localPosition = new Vector2(i * 1080, 0);

        FindObjectOfType<StartButton>().editorPanel = rtpe;

        buttons.Add(rtpe);

        i++;

        foreach (var file in files) {
            var panel = Instantiate(songPanel, scrollPanel.transform);

            var rt = panel.GetComponent<RectTransform>();
            rt.localPosition = new Vector2(i * 1080, 0);

            var rawLevelName = Path.GetFileName(file).Replace(".json", "");
            var levelName = rawLevelName.First().ToString().ToUpper() + rawLevelName.Substring(1);

            string splitString = Regex.Replace(
                    Regex.Replace(
                        levelName,
                        @"(\P{Ll})(\P{Ll}\p{Ll})",
                        "$1 $2"
                    ),
                    @"(\p{Ll})(\P{Ll})",
                    "$1 $2"
                );

            panel.GetComponentInChildren<Text>().text = splitString;
            panel.GetComponentInChildren<SongOption>().level = file;
            panel.GetComponentInChildren<SongOption>().standardLevelImage.gameObject.SetActive(!file.StartsWith(DataManagement.StandardLevels));

            buttons.Add(rt);

            i++;
        }

        scrollRectSnap.SetButtons(buttons.ToArray());

        scrollRectSnap.GoToIndex(1);
    }
}
