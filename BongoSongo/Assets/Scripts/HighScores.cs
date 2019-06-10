using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScores : MonoBehaviour {
    
    [System.Serializable]
    public struct HighScore {
        public string levelFile;
        public int score;
    }

    public static int SaveNewHighScore(Level level, int score) {
        string filePath = Path.Combine(Application.persistentDataPath,"highscores", "highscores.json");
        string folder = Path.Combine(Application.persistentDataPath, "highscores");

        if (!File.Exists(filePath)) {
            DirectoryInfo di = Directory.CreateDirectory(folder);
            Debug.Log($"The directory was created successfully at {Directory.GetCreationTime(folder)}.");
        }

        var highScores = LoadScores();

        if (!highScores.ContainsKey(level.path)) {
            highScores[level.path] = score;
        } else if (score > highScores[level.path]) {
            highScores[level.path] = score;
        }

        var json = JsonConvert.SerializeObject(highScores);

        using (var fileStream = File.Open(filePath, FileMode.OpenOrCreate)) {
            using (StreamWriter sw = new StreamWriter(fileStream)) {
                foreach (char c in json) sw.Write(c);
            }
        }
        
        return highScores[level.path];
    }

    public static Dictionary<string,int> LoadScores() {
        string filePath = Path.Combine(Application.persistentDataPath,"highscores", "highscores.json");

        if (File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);

            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);

            deserialized = deserialized ?? new Dictionary<string, int>();

            return deserialized;
        } else {
            return new Dictionary<string, int>();
        }
    }

}
