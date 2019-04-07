using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenScore : MonoBehaviour {
    void Start() {
        var interScene = FindObjectOfType<InterScene>();

        if (interScene == null) return;

        GetComponent<UnityEngine.UI.Text>().text = "" + interScene.score;

        Destroy(interScene.gameObject);
    }
}
