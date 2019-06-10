using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScreenTitle : MonoBehaviour{
    void Start() {
        GetComponent<UnityEngine.UI.Text>().text = "" + InterScene.Instance.level?.name;
    }
}
