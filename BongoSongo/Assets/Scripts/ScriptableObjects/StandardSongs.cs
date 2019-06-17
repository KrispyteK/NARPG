
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Standard Songs", order = 1)]
public class StandardSongs : ScriptableObject {

    public Song[] songs;

    [System.Serializable]
    public class Song {
        public AudioClip audioClip;
        public Sprite icon;
        public int bpm;

        [HideInInspector] public string audioClipPath;
        [HideInInspector] public string iconPath;
    }

#if UNITY_EDITOR
    void OnValidate() {
        foreach (var song in songs) {
            var audioPath = UnityEditor.AssetDatabase.GetAssetPath(song.audioClip);
            var regex = new System.Text.RegularExpressions.Regex(@"^(Assets/Resources/)|(.mp3|.wav)$");
            audioPath = regex.Replace(audioPath, "");

            song.audioClipPath = audioPath;

            //var iconPath = UnityEditor.AssetDatabase.GetAssetPath(song.icon);
            //regex = new System.Text.RegularExpressions.Regex(@"^(Assets/Resources/)|(.png)$");
            //iconPath = regex.Replace(audioPath, "");

            //song.iconPath = audioPath;
        }

        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}