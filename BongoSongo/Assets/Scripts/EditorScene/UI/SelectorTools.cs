using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectorTools : MonoBehaviour {

    public List<GameObject> extraTools;

    public void ResetTools () {
        foreach (var tool in extraTools) {
            Destroy(tool);
        }

        extraTools.Clear();
    }
}
