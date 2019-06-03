using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using UnityEngine.Networking;

[System.Serializable]
public class Level : ScriptableObject {
    //public string name;
    public string description;
    public float bpm;
    public Song song;

    public List<SpawnInfo> spawnInfo = new List<SpawnInfo>();

    static Level () {
        BetterStreamingAssets.Initialize();
    }

    public Level () {
        name = "unnamed";
        bpm = 128;
    }

    public override string ToString() {
        return $"Level \nname:{name}\ndescription:{description}\nbpm:{bpm}";
    }

    public static void Save(Level level) {
        var fileName = Application.persistentDataPath;
        fileName = Path.Combine(fileName, $"{level.name}.level");

        var json = JsonConvert.SerializeObject(level);

        Debug.Log(json);

        using (StreamWriter outputFile = new StreamWriter(fileName)) {
            foreach (char c in json) outputFile.Write(c);
        }


        Debug.Log("Level saved to: " + fileName);
    }

    public static Level Load(string file) {
        Level deserialized;
        string json;

        string path = Path.Combine(Application.persistentDataPath, file);

        Debug.Log("Loading level from: " + path + "...");

        if (!File.Exists(path)) {
            Debug.LogErrorFormat("Streaming asset not found: {0}", path);
            return null;
        }

        json = File.ReadAllText(path);

        deserialized = JsonConvert.DeserializeObject<Level>(json);

        Debug.Log("Level loaded succesfully!");

        return deserialized;
    }
}
