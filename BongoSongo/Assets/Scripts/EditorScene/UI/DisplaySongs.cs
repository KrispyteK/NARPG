using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplaySongs : MonoBehaviour {
    public GameObject levelButton;
    public Transform contentPanel;
    public AudioClip[] songs;
    public List<string> songPaths = new List<string>();

    public Button okButton;

    private List<SongButton> songButtons = new List<SongButton>();

    private struct SongButton {
        public AudioClip song;
        public Button button;
    }

#if UNITY_EDITOR
    void OnValidate() {
        songPaths.Clear();

        foreach (var clip in songs) {
            var path = UnityEditor.AssetDatabase.GetAssetPath(clip);
            var regex = new System.Text.RegularExpressions.Regex(@"^(Assets/Resources/)|(.mp3|.wav)$");
            path = regex.Replace(path, "");

            songPaths.Add(path);
        }
    }
#endif

    void Start() {
        GenerateButtons();

        okButton.onClick.AddListener(Ok);
    }

    void Ok () {
        //foreach (var songButton in songButtons) {
        //    print(songButton.button.IsHighlighted());
        //}
    }

    public void GenerateButtons() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        songButtons.Clear();

        foreach (var song in songs) {
            var button = Instantiate(levelButton, contentPanel);

            button.GetComponentInChildren<Text>().text = song.name;

            var buttonComponent = button.GetComponent<Button>();

            songButtons.Add(new SongButton {
                button = buttonComponent,
                song = song
            });
        }
    }
}
