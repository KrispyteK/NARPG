using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class DisplayLevels : MonoBehaviour {

    public GameObject levelButton;
    public Transform contentPanel;
    public Button okButton;
    public GameObject panel;

    private List<SelectableButton> levelButtons = new List<SelectableButton>();
    private List<string> levelFiles = new List<string>();
    private SelectableButton.SelectableButtonPool selectableButtonPool = new SelectableButton.SelectableButtonPool();

    void Start() {
        GenerateButtons();

        okButton.onClick.AddListener(Ok);
    }

    void Ok () {
        var i = 0;

        foreach (var button in levelButtons) {
            if (button.IsSelected) {
                EditorManager.instance.Load(levelFiles[i]);

                panel.SetActive(false);
 
                break;
            }

            i++;
        }
    }

    public void GenerateButtons () {
        foreach (Transform child in contentPanel) {
            Destroy(child.gameObject);
        }

        levelButtons.Clear();
        levelFiles.Clear();

        var files = new List<string>();

        files.AddRange(Directory.GetFiles(Application.persistentDataPath, "*.level", SearchOption.AllDirectories));
 
        foreach (var file in files) {
            levelFiles.Add(file);

            var button = Instantiate(levelButton, contentPanel);

            button.GetComponentInChildren<Text>().text = Path.GetFileName(file).Replace(".level","");

            var buttonComponent = button.GetComponent<SelectableButton>();

            buttonComponent.AddToPool(selectableButtonPool);

            levelButtons.Add(buttonComponent);
        }
    }
}
