using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class BeatMapWindow : EditorWindow {

    private static SpawnManager spawnManager;
    private static GameManager gameManager;

    private int modifyingIndex;
    private bool isModifying;
    private Vector2 offset;

    private float Size => Screen.width * gameManager.buttonSize;

    [MenuItem("Mapping/Window")]
    public static void CreateMenu() {
        GetWindow<BeatMapWindow>();

        GetManagers();
    }

    private static void GetManagers() {
        spawnManager = FindObjectOfType<SpawnManager>();
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update() {
        if (isModifying) Repaint();
    }

    public void OnGUI() {
        if (spawnManager == null) GetManagers();

        var e = Event.current;
        var controlID = GUIUtility.GetControlID(FocusType.Passive);

        switch (e.GetTypeForControl(controlID)) {
            case EventType.MouseUp:
                isModifying = false;

                e.Use();
                break;

            case EventType.MouseDown:
                CheckClick();

                e.Use();
                break;
        }

        DisplayBeats();
        ModifyButton();
    }

    private void ModifyButton() {
        if (!isModifying) return;

        var position = (Event.current.mousePosition + new Vector2(Size, Size) / 2 + offset) / new Vector2(Screen.width, Screen.height);

        spawnManager.spawnInfo[modifyingIndex].position = new Vector2(Mathf.Clamp01(position.x), Mathf.Clamp01(position.y));
    }

    private void CheckClick() {
        var mousePos = Event.current.mousePosition;

        for (int i = 0; i < spawnManager.spawnInfo.Count; i++) {
            var spawnInfo = spawnManager.spawnInfo[i];
            var position = new Vector2(
                    spawnInfo.position.x * Screen.width - Size / 2,
                    spawnInfo.position.y * Screen.height - Size / 2
                );

            var rect = new Rect(position, new Vector2(Size, Size));

            if (rect.Contains(mousePos)) {
                isModifying = true;
                modifyingIndex = i;
                offset = position - mousePos;

                return;
            }
        }
    }

    private void DisplayBeats() {
        for (int i = 0; i < spawnManager.spawnInfo.Count; i++) {
            var spawnInfo = spawnManager.spawnInfo[i];
            var position = new Vector2(
                    spawnInfo.position.x * Screen.width - Size / 2,
                    spawnInfo.position.y * Screen.height - Size / 2
                );

            GUI.Box(new Rect(
                    position,
                    new Vector2(Size, Size)
                ), "");
        }
    }
}
