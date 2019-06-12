using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteLevel : MonoBehaviour {

    public DisplayLevels displayLevels;

    public void Delete () {
        Level.Delete(displayLevels.GetLevelFileFromSelected());
    }
}
