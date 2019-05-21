
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SongStandard : ISong {
    public string audioClipFile;

    public AudioClip GenerateClip() {
        return Resources.Load<AudioClip>(audioClipFile);
    }
}
