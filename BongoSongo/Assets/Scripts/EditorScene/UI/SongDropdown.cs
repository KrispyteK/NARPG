using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongDropdown : MonoBehaviour {
    public AudioClip selectedSong;
    public AudioClip[] songs;
    public Dropdown dropdown;

    private TimelineDrawer timelineDrawer;


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
        selectedSong = songs[num];

#if UNITY_EDITOR
        var path = UnityEditor.AssetDatabase.GetAssetPath(selectedSong);

        var regex = new System.Text.RegularExpressions.Regex(@"^(Assets/Resources/)|(.mp3|.wav)$");

        path = regex.Replace(path,"");

        if (EditorManager.instance.level.song is SongStandard stdSong) {
            stdSong.audioClipFile = path;
        } else if (EditorManager.instance.level.song == null) {
            EditorManager.instance.level.song = new SongStandard {
                audioClipFile = path
            };
        }
#endif

        timelineDrawer.RedrawTimeline();
    }
}
