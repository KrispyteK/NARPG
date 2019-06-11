using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandardFolders : MonoBehaviour {

    static string[] standardFolders = new[] {
            "Levels",
            "HighScores"
        };

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod() {
        foreach (var folder in standardFolders) {
            var path = Path.Combine(Application.persistentDataPath, folder);

            if (!Directory.Exists(path)) {
                DirectoryInfo di = Directory.CreateDirectory(path);
                Debug.Log($"{folder} folder was created successfully at {Directory.GetCreationTime(path)}.");
            }
        }
    }
}
