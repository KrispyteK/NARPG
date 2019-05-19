using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class Level {
    public string name;
    public string description;

    public List<SpawnInfo> spawnInfo = new List<SpawnInfo>();

    public override string ToString() {      
        return $"Level \n{name}\n{description}\n{string.Join(",",spawnInfo)}";
    }

    public static void Save (Level level) {
        var fileName = Application.persistentDataPath;
        fileName = Path.Combine(fileName, $"{level.name}.level");

        using (var stream = File.Open(fileName, FileMode.Create)) { 
            Debug.Log("Level saved to: " + fileName);

            var bf = new BinaryFormatter();

            bf.Serialize(stream, level);
        }

        using (var stream = File.Open(fileName, FileMode.Open)) {
            Debug.Log(stream);

            var bf = new BinaryFormatter();

            var deserialized = (Level)bf.Deserialize(stream);

            Debug.Log(deserialized.name);
        }
    }
}
