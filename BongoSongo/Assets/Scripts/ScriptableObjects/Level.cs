using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using FullSerializer;
using Newtonsoft.Json;

[System.Serializable]
public class Level : ScriptableObject {
    //public string name;
    public string description;
    public float bpm;
    public Song song;

    public List<SpawnInfo> spawnInfo = new List<SpawnInfo>();

    public Level () {
        name = "unnamed";
        bpm = 128;
    }

    public override string ToString() {
        return $"Level \nname:{name}\ndescription:{description}\nbpm:{bpm}";
    }

    public static void Save(Level level) {
        var fileName = Application.streamingAssetsPath;
        fileName = Path.Combine(fileName, $"{level.name}.level");

        var json = JsonConvert.SerializeObject(level);

        Debug.Log(json);

        using (StreamWriter outputFile = new StreamWriter(fileName)) {
            foreach (char c in json) outputFile.Write(c);
        }

        Debug.Log("Level saved to: " + fileName);
    }

    public static Level Load(string path) {
        Debug.Log("Loading level from: " + path  + "...");

        Level deserialized;

        using (StreamReader sr = new StreamReader(path)) {
            // Read the stream to a string, and write the string to the console.
            var json = sr.ReadToEnd();

            deserialized = JsonConvert.DeserializeObject<Level>(json);
        }

        Debug.Log("Level loaded succesfully!");

        return deserialized;
    }
}
