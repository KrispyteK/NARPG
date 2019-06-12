using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public class DisplayLevels : MonoBehaviour {

    public GameObject newWindow;
    public GameObject levelButton;
    public Transform contentPanel;
    public Button okButton;
    public Button deleteButton;
    public GameObject panel;

    private List<SelectableButton> levelButtons = new List<SelectableButton>();
    private List<string> levelFiles = new List<string>();
    private SelectableButton.SelectableButtonPool selectableButtonPool;

    void Start() {
        selectableButtonPool = new SelectableButton.SelectableButtonPool {
            button = okButton
        };

        GenerateButtons();

        okButton.onClick.AddListener(Ok);
    }

    public string GetLevelFileFromSelected () {
        string file = "";
        var i = 0;

        foreach (var button in levelButtons) {
            if (button.IsSelected) {
                file = levelFiles[i];
                break;
            }

            i++;
        }

        return file;
    }

    void Update () {
        okButton.interactable = selectableButtonPool.selected != null;
        deleteButton.interactable = selectableButtonPool.selected != null;
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
        Level.OnBeforeSceneLoadRuntimeMethod();

        foreach (Transform child in contentPanel) {
            Destroy(child.gameObject);
        }

        levelButtons.Clear();
        levelFiles.Clear();

        var files = new List<string>(Directory.GetFiles(DataManagement.Levels, "*.json", SearchOption.AllDirectories));

        foreach (var file in files) {

#if !UNITY_EDITOR
            var level = Level.LoadFromFullPath(file);

            if (level.isStandard) continue;
#endif

            levelFiles.Add(file);

            var button = Instantiate(levelButton, contentPanel);

            button.GetComponentInChildren<Text>().text = Path.GetFileName(file).Replace(".json","");

            var buttonComponent = button.GetComponent<SelectableButton>();

            buttonComponent.AddToPool(selectableButtonPool);

            levelButtons.Add(buttonComponent);
        }

        GetComponent<Window>().closeButton.interactable = EditorManager.instance.hasLevelLoaded;

        if (levelFiles.Count == 0) {
            GetComponent<Window>().closeButton.interactable = false;

            newWindow.GetComponent<Window>().closeButton.interactable = false;
            newWindow.GetComponent<Window>().Open();
            FindObjectOfType<EditorTutorial>().NoLevels.SetActive(true);
        }
    }
}
