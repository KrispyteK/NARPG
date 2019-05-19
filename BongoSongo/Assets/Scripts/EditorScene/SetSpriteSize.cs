using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpriteSize : MonoBehaviour {

    void Start() {
        transform.localScale = CameraTransform.Scale(Vector3.one * InterScene.Instance.gamePlaySettings.indicatorScale);
    }
}
