using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorDrawer : MonoBehaviour {

    public Drawer drawer;
    public GameObject indicatorToolPrefab;

    void Start() {
        foreach (var indicator in EditorManager.instance.editorPrefabs) {
            var instance = Instantiate(indicatorToolPrefab, transform);

            instance.GetComponentInChildren<IndicatorTool>().indicator = indicator.indicator;
            instance.GetComponentInChildren<Image>().sprite = indicator.preview;
            instance.GetComponentInChildren<Button>().onClick.AddListener(() => {
                EditorManager.instance.currentPrefab = indicator;
                drawer.isOpened = false;
            });
        }
    }

}
