using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class Level {
    public string name;
    public string description;
    public float bpm;

    public List<SpawnInfo> spawnInfo = new List<SpawnInfo>();

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

        using (var stream = File.Open(fileName, FileMode.Create)) {
            Debug.Log("Level saved to: " + fileName);

            var bf = new BinaryFormatter();

            bf.Serialize(stream, level);
        }
    }

    public static Level Load(string path) {
        Level deserialized;

        using (var stream = File.Open(path, FileMode.Open)) {
            Debug.Log("Loading level from: " + path);

            var bf = new BinaryFormatter();

            deserialized = (Level)bf.Deserialize(stream);

            Debug.Log(deserialized);
        }

        return deserialized;
    }
}
