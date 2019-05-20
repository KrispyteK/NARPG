using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongDropdown : MonoBehaviour {

    public AudioClip selectedSong;
    public AudioClip[] songs;

    public Dropdown dropdown;

    void Start () {
        dropdown.ClearOptions();

        var options = new List<string>();

        foreach (var clip in songs) {
            options.Add(clip.name);
        }

        dropdown.AddOptions(options);

        dropdown.onValueChanged.AddListener(OnChanged);

        OnChanged(0);
    }

    public void OnChanged (int num) {
        selectedSong = songs[num];

        if (EditorManager.instance.level.song is SongStandard stdSong) {
            stdSong.clip = selectedSong;
        } else if (EditorManager.instance.level.song == null) {
            EditorManager.instance.level.song = new SongStandard {
                clip = selectedSong
            };
        }

        TimelineDrawer.instance.RedrawTimeline();
    }
}
