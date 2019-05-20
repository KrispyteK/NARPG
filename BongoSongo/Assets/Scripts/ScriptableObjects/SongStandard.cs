
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SongStandard : ISong {
    public string file;

    public AudioClip GenerateClip() {
        throw new System.NotImplementedException();
    }
}
