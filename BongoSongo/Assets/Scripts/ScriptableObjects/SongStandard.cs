
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SongStandard : Song {
    public string audioClipFile;

    public override AudioClip GenerateClip() {
        return Resources.Load<AudioClip>(audioClipFile);
    }
}
