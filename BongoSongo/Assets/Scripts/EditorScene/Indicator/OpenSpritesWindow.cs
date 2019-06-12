using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenSpritesWindow : MonoBehaviour {

    public GameObject window;

    public void OpenWindow () {
        var instance = Instantiate(window, transform.root);

        instance.transform.SetSiblingIndex(transform.parent.GetSiblingIndex() + 1);
    }
}
