
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SongStandard : ISong {
    public AudioClip clip;

    public AudioClip GenerateClip() {
        return clip;
    }
}
