using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class Song {

    public string clipString;

    public virtual AudioClip GenerateClip() {
        return Resources.Load<AudioClip>(clipString);
    }
}