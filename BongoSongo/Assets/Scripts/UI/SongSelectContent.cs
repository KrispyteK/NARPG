using System.Linq;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSelectContent : MonoBehaviour {

    public Sprite editorSprite;
    public GameObject editorPanel;
    public GameObject songPanel;
    public GameObject scrollPanel;
    public GameObject loadingUI;

    private ScrollRectSnap scrollRectSnap;
    private StandardSongs standardSongs;
    private List<RectTransform> buttons = new List<RectTransform>();

    void Start() {
        scrollRectSnap = GetComponent<ScrollRectSnap>();

        standardSongs = Resources.Load<StandardSongs>("Settings/StandardSongs");

        GenerateButtons();
    }

    private string SplitCamelCase(string input) {
        return Regex.Replace(
                    Regex.Replace(
                        input,
                        @"(\P{Ll})(\P{Ll}\p{Ll})",
                        "$1 $2"
                    ),
                    @"(\p{Ll})(\P{Ll})",
                    "$1 $2"
                );
    }

    public void GenerateButtons() {
        scrollRectSnap.canScroll = false;

        foreach (Transform child in scrollPanel.transform) {
            Destroy(child.gameObject);
        }

        var files = new List<string>(Directory.GetFiles(DataManagement.StandardLevels, "*.json", SearchOption.AllDirectories));
        files.AddRange(Directory.GetFiles(DataManagement.Levels, "*.json", SearchOption.AllDirectories));
        files = files.OrderByDescending(f => Path.GetFileName(f)).Reverse().ToList();

        var panelEditor = Instantiate(editorPanel, scrollPanel.transform);
        panelEditor.GetComponent<Image>().sprite = editorSprite;

        var rtpe = panelEditor.GetComponent<RectTransform>();
        rtpe.localPosition = new Vector2(0 * 1080, 0);

        FindObjectOfType<StartButton>().editorPanel = rtpe;

        buttons.Add(rtpe);

        StartCoroutine(LevelLoadRoutine(files));
    }

    private GameObject CreateLevelButton(Level level, string file, int i) {
        var name = level.name;
        var iconSprite = standardSongs.songs.ToList().Find(x => x.audioClipPath == level.song.clipString).icon;

        var panel = Instantiate(songPanel, scrollPanel.transform);

        var rt = panel.GetComponent<RectTransform>();
        rt.localPosition = new Vector2(i * 1080, 0);

        panel.GetComponent<Image>().sprite = iconSprite;
        panel.GetComponentInChildren<Text>().text = SplitCamelCase(name);
        panel.GetComponentInChildren<SongOption>().level = file;
        panel.GetComponentInChildren<SongOption>().standardLevelImage.gameObject.SetActive(!file.StartsWith(DataManagement.StandardLevels));

        return panel;
    }

    private IEnumerator LevelLoadRoutine(List<string> files) {
        yield return new WaitForEndOfFrame();

        int loaded = 0;
        int loadCount = files.Count;

        while (loaded < files.Count) {
            Level level = Level.LoadFromFullPath(files[loaded]);

            GameObject panel = CreateLevelButton(level, files[loaded], loaded + 1);

            buttons.Add(panel.GetComponent<RectTransform>());

            loaded++;

            yield return new WaitForEndOfFrame();
        }

        scrollRectSnap.SetButtons(buttons.ToArray());

        scrollRectSnap.GoToIndex(1);

        scrollRectSnap.canScroll = true;

        loadingUI.SetActive(false);
    }
}
