using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpriteSize : MonoBehaviour {

    public Transform sprite;

    void Start() {
        sprite.localScale = CameraTransform.Scale(Vector3.one * InterScene.Instance.gamePlaySettings.indicatorScale);
    }
}
