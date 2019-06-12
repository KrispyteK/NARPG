using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorSpriteSelector : MonoBehaviour {

    public RectTransform window;
    public int index;

    void Start() {
        var button = GetComponent<Button>();

        button.onClick.AddListener(Action);
    }

    void Action () {
        EditorManager.instance.SetIndicatorSpriteIndex(index);
        window.gameObject.SetActive(false);

        var selectedTools = FindObjectOfType<SelectorTools>();
        selectedTools.ResetTools();
        selectedTools.gameObject.SetActive(false);
    }
}
