using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongDropdown : MonoBehaviour {
    public AudioClip selectedSong;
    public AudioClip[] songs;
    public Dropdown dropdown;

    private TimelineDrawer timelineDrawer;
    public List<string> songPaths = new List<string>();

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

    void Start () {
        dropdown.ClearOptions();

        var options = new List<string>();

        foreach (var clip in songs) {
            options.Add(clip.name);
        }

        dropdown.AddOptions(options);

        dropdown.onValueChanged.AddListener(OnChanged);

        timelineDrawer = FindObjectOfType<TimelineDrawer>();

        OnChanged(0);
    }

    public void OnChanged (int num) {
        var path = songPaths[num];

        if (EditorManager.instance.level != null) {
            if (EditorManager.instance.level.song is Song song) {
                song.clipString = path;
            } else if (EditorManager.instance.level.song == null) {
                EditorManager.instance.level.song = new Song {
                    clipString = path
                };
            }
        }

        EditorManager.instance.LoadSong();
    }
}
