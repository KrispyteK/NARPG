using System.Linq;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSelectContent : MonoBehaviour {

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

        foreach (var file in files) {
            var panel = Instantiate(songPanel, scrollPanel.transform);

            var rt = panel.GetComponent<RectTransform>();
            rt.localPosition = new Vector2(i * 1080, 0);

            var rawLevelName = Path.GetFileName(file).Replace(".json", "");
            var levelName = rawLevelName.First().ToString().ToUpper() + rawLevelName.Substring(1);

            panel.GetComponentInChildren<Text>().text = levelName;
            panel.GetComponentInChildren<SongOption>().level = rawLevelName;

            buttons.Add(rt);

            i++;
        }

        scrollRectSnap.SetButtons(buttons.ToArray());
    }
}
