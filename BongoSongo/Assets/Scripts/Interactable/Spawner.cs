using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[System.Serializable]
public struct SpawnInfo {
    public int beat;
    public int bar;

    public Vector2 position;
}

public class Spawner : MonoBehaviour {
    public GameObject hitThis;
    public List<SpawnInfo> spawnInfo;

    public void Spawn(SpawnInfo spawn) {
        var button = Instantiate(hitThis, CameraTransform.ScreenPointToWorldScaled(spawn.position), Quaternion.identity);
        var interactable = button.GetComponent<Interactable>();
    }
}

[CustomEditor(typeof(Spawner))]
public class ObjectBuilderEditor : Editor {
    public override void OnInspectorGUI() {
        var spawner = (Spawner)target;

        // Insert tool

        var insertButton = GUILayout.Button("Insert", GUILayout.MaxWidth(10));

        if (GUILayout.Button("Build Object")) {
            //myScript.BuildObject();
        }

        DrawDefaultInspector();

    }
}