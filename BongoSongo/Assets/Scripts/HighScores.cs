using Newtonsoft.Json;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScores : MonoBehaviour {
    
    static string FilePath => Path.Combine(DataManagement.HighScores, "highscores.json");

    [System.Serializable]
    public struct HighScore {
        public string levelFile;
        public int score;
    }

    public static int SaveNewHighScore(Level level, int score) {
        var highScores = LoadScores();

        if (!highScores.ContainsKey(level.path)) {
            highScores[level.path] = score;
        } else if (score > highScores[level.path]) {
            highScores[level.path] = score;
        }

        var json = JsonConvert.SerializeObject(highScores);

        using (var fileStream = File.Open(FilePath, FileMode.OpenOrCreate)) {
            using (StreamWriter sw = new StreamWriter(fileStream)) {
                foreach (char c in json) sw.Write(c);
            }
        }
        
        return highScores[level.path];
    }

    public static Dictionary<string,int> LoadScores() {
        if (File.Exists(FilePath)) {
            string json = File.ReadAllText(FilePath);

            var deserialized = JsonConvert.DeserializeObject<Dictionary<string, int>>(json);

            deserialized = deserialized ?? new Dictionary<string, int>();

            return deserialized;
        } else {
            return new Dictionary<string, int>();
        }
    }

}
