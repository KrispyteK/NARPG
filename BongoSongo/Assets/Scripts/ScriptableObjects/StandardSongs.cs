
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Standard Songs", order = 1)]
public class StandardSongs : ScriptableObject {

    public Song[] songs;

    [System.Serializable]
    public class Song {
        public AudioClip audioClip;
        public int bpm;
        public string audioClipPath;
    }

#if UNITY_EDITOR
    void OnValidate() {
        foreach (var song in songs) {
            var path = UnityEditor.AssetDatabase.GetAssetPath(song.audioClip);
            var regex = new System.Text.RegularExpressions.Regex(@"^(Assets/Resources/)|(.mp3|.wav)$");
            path = regex.Replace(path, "");

            song.audioClipPath = path;
        }
    }
#endif
}