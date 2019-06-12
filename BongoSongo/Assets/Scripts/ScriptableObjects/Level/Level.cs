using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

[System.Serializable]
public class Level {
    public string name;
    public string description;
    public string path;
    public float bpm;
    public bool isStandard;
    public Song song;

    public List<SpawnInfo> spawnInfo = new List<SpawnInfo>();

    public static string Folder =>
#if (DEBUG && !UNITY_EDITOR)
        "file:///" +
#endif
        $"{Application.persistentDataPath}";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnBeforeSceneLoadRuntimeMethod() {
        Debug.Log("Writing levels to device...");

        var standardLevels = Resources.Load<StandardLevels>("Levels/StandardLevels");

        Debug.Log(standardLevels);

        foreach (var levelFile in standardLevels.levels) {
            string path = Path.Combine(Folder, "Levels", $"{levelFile.name}.json");

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
        level.isStandard = true;

        string path = Path.Combine(Application.dataPath, "Resources", "Levels", $"{level.name}.json");
#else
        string path = Path.Combine(Folder, $"{level.name}.json");
#endif

        Debug.Log("Saving level to: " + path);

        level.path = Path.Combine(Application.persistentDataPath, "Levels", $"{level.name}.json");

        var json = JsonConvert.SerializeObject(level, Formatting.Indented);

        using (StreamWriter outputFile = new StreamWriter(path)) {
            foreach (char c in json) outputFile.Write(c);
        }

#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();

        var textAsset = Resources.Load<TextAsset>($"Levels/{level.name}");

        var standardLevels = Resources.Load<StandardLevels>("Levels/StandardLevels");

        if (!standardLevels.levels.Contains(textAsset)) {
            standardLevels.levels.Add(textAsset);
        }

        UnityEditor.EditorUtility.SetDirty(standardLevels);
#endif

        Debug.Log("Succesfully saved level!");
    }

    public static Level LoadFromFullPath (string file) {
        string json = File.ReadAllText(file);

        Level deserialized = JsonConvert.DeserializeObject<Level>(json);

        return deserialized;
    }
    
    public static Level Load(string file) {
        string filePath = Path.Combine(Folder, "Levels", file);

        string json = File.ReadAllText(filePath);

        Level deserialized = JsonConvert.DeserializeObject<Level>(json);

        return deserialized;
    }
}
