using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

[System.Serializable]
public class Level : ScriptableObject {
    //public string name;
    public string description;
    public float bpm;
    public Song song;

    public List<SpawnInfo> spawnInfo = new List<SpawnInfo>();

    public static string FolderEditor => Path.Combine(Application.dataPath, "Resources/Levels/Standard");
    public static string FolderRelease => Path.Combine(Application.persistentDataPath, "Levels");

    public static string Folder => $"{Application.persistentDataPath}/Levels";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod() {
        Debug.Log("Before scene loaded");
        BetterStreamingAssets.Initialize();

        Debug.Log("Writing levels to device...");

        var standardLevels = Resources.FindObjectsOfTypeAll<StandardLevels>().First();

        if (!Directory.Exists(Folder)) Directory.CreateDirectory(Folder);

        foreach (var levelFile in standardLevels.levels) {
            string levelName = Regex.Replace(levelFile.name, "^StandardLevel", "");

            string path = Path.Combine(Folder, $"{levelName}.json");

            File.WriteAllText(path, levelFile.text);

            Debug.Log("Level saved to: " + path);
        }

        Debug.Log("Writing levels complete!");
    }

    public Level() {
        name = "unnamed";
        bpm = 128;
    }

    public override string ToString() {
        return $"Level \nname:{name}\ndescription:{description}\nbpm:{bpm}";
    }

    public static void Save(Level level) {

#if UNITY_EDITOR
        string path = Path.Combine(Application.dataPath, "Resources", "Levels", $"{level.name}.json");
#else
        string path = Path.Combine(Folder, $"{level.name}.json");
#endif

        Debug.Log("Saving level to: " + path);

        var json = JsonConvert.SerializeObject(level);

        using (StreamWriter outputFile = new StreamWriter(path)) {
            foreach (char c in json) outputFile.Write(c);
        }

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
#endif

        Debug.Log("Succesfully saved level!");
    }



    public static Level Load(string file) {
        string filePath = Path.Combine(Folder, file);

        Debug.Log($"Loading level {filePath}");

        string json = File.ReadAllText(filePath);

        Level deserialized = JsonConvert.DeserializeObject<Level>(json);

        return deserialized;
    }
}
