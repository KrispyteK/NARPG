using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class DisplayLevels : MonoBehaviour {

    public GameObject levelButton;
    public Transform contentPanel;

    void Start() {
        GenerateButtons();
    }

    public void GenerateButtons () {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        var files = new List<string>();

        files.AddRange(Directory.GetFiles(Application.streamingAssetsPath, "*.level", SearchOption.AllDirectories));
 
        foreach (var file in files) {
            var button = Instantiate(levelButton, contentPanel);

            button.GetComponentInChildren<Text>().text = Path.GetFileName(file).Replace(".level","");
        }
    }
}
