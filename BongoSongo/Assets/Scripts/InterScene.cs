using UnityEngine;
using UnityEngine.SceneManagement;

public class InterScene : Singleton<InterScene> {
    //private static InterScene _instance;

    public Level level;
    public int score;
    public GamePlaySettings gamePlaySettings;

    protected override bool DontDestroyOnLoad => true;

    protected override void OnInitiate() {
        gameObject.name = "InterScene";
        gamePlaySettings = Resources.Load<GamePlaySettings>("Settings/GamePlaySettings");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        var spawnManager = FindObjectOfType<SpawnManager>();

        print(spawnManager);

        if (spawnManager) {
            spawnManager.spawnInfo = level.spawnInfo;
        }
    }
}
