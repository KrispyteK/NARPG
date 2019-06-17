using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataManagement : MonoBehaviour {

    public static string Levels => Path.Combine(Application.persistentDataPath, "Levels");
    public static string HighScores => Path.Combine(Application.persistentDataPath, "HighScores");
    public static string StandardLevels => Path.Combine(Application.persistentDataPath, "StandardLevels");

    static string[] standardFolders = new[] {
            "Levels",
            "HighScores",
            "StandardLevels"
        };

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void CreateStandardDirectories() {
        foreach (var folder in standardFolders) {
            var path = Path.Combine(Application.persistentDataPath, folder);

            if (!Directory.Exists(path)) {
                DirectoryInfo di = Directory.CreateDirectory(path);
                Debug.Log($"{folder} folder was created successfully at {Directory.GetCreationTime(path)}.");
            }
        }
    }

    public static void CheckDirectories () {
        CreateStandardDirectories();
    }
}
