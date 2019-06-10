using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenScore : MonoBehaviour {
    void Start() {
        GetComponent<UnityEngine.UI.Text>().text = "" + InterScene.Instance.score;
    }
}
