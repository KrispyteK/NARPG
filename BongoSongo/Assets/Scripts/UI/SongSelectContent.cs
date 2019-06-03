using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongSelectContent : MonoBehaviour {

    public GameObject songPanel;

    void Start() {
        GenerateButtons();
    }

    public void GenerateButtons() {
        foreach (Transform child in transform) {
            Destroy(child.gameObject);
        }

        var files = new List<string>();

        files.AddRange(Directory.GetFiles(Application.persistentDataPath, "*.level", SearchOption.AllDirectories));

        foreach (var file in files) {
            var panel = Instantiate(songPanel, transform);

            panel.GetComponentInChildren<Text>().text = Path.GetFileName(file).Replace(".level", "");
        }
    }
}
