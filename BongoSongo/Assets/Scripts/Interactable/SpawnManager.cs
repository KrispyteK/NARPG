using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public class SpawnInfo {
    public int beat;
    public int bar;

    public Vector2 position;
}

public class SpawnManager : MonoBehaviour {
    public GameObject hitThis;
    public List<SpawnInfo> spawnInfo;

    public void Spawn(SpawnInfo spawn) {
        var button = Instantiate(hitThis, CameraTransform.ScreenPointToWorldScaled(spawn.position), Quaternion.identity);
        var interactable = button.GetComponent<Interactable>();
    }
}

//[CustomEditor(typeof(Spawner))]
//public class SpawnerEditor : Editor {

//    private Spawner spawner;
//    private Tool currentTool;

//    private enum Tool {
//        None,
//        Insert,
//        Remove
//    }

//    void OnEnable() {
//        spawner = (Spawner)target;

//        currentTool = Tool.None;
//    }

//    public override void OnInspectorGUI() {
//        ToolBar();

//        GUILayout.FlexibleSpace();

//        DrawDefaultInspector();
//    }

//    private void ToolBar () {
//        GUILayout.Label("Tools");

//        GUILayout.BeginHorizontal();

//        bool insertButton, removeButton;

//        switch (currentTool) {
//            case Tool.None:
//                insertButton = GUILayout.Button("Insert", GUILayout.MaxWidth(Screen.width / 2));
//                removeButton = GUILayout.Button("Remove", GUILayout.MaxWidth(Screen.width / 2));

//                if (insertButton) currentTool = Tool.Insert;
//                if (removeButton) currentTool = Tool.Remove;

//                break;
//            case Tool.Insert:
//            case Tool.Remove:
//                GUILayout.Label(currentTool.ToString());

//                break;
//            default:
//                break;
//        }

//        GUILayout.EndHorizontal();

//        switch (currentTool) {
//            case Tool.None:
//                break;
//            case Tool.Insert:
//                InsertTool();
//                break;
//            case Tool.Remove:
//                RemoveTool();
//                break;
//            default:
//                break;
//        }
//    }

//    private void InsertTool () {
//        GUILayout.Space(10);

//        EditorGUILayout.IntField("At", 0);

//        GUILayout.Space(10);

//        GUILayout.BeginHorizontal();

//        var insertButton = GUILayout.Button("Insert", GUILayout.MaxWidth(Screen.width / 2));

//        if (GUILayout.Button("Cancel", GUILayout.MaxWidth(Screen.width / 2))) currentTool = Tool.None;

//        GUILayout.EndHorizontal();
//    }

//    private void RemoveTool() {
//        GUILayout.Space(10);

//        EditorGUILayout.IntField("At", 0);

//        GUILayout.Space(10);

//        GUILayout.BeginHorizontal();

//        var insertButton = GUILayout.Button("Remove", GUILayout.MaxWidth(Screen.width / 2));

//        if (GUILayout.Button("Cancel", GUILayout.MaxWidth(Screen.width / 2))) currentTool = Tool.None;

//        GUILayout.EndHorizontal();
//    }
//}