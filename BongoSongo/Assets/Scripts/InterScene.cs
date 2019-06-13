using UnityEngine;
using UnityEngine.SceneManagement;

public class InterScene : Singleton<InterScene> {
    public Level level;
    public int score;
    public GamePlaySettings gamePlaySettings;

    protected override bool DontDestroyLoad => true;

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod() {
        Debug.Log("Creating interscene object...");

        var interScene = InterScene.Instance;
    }

    private void Awake () {
        DontDestroyOnLoad(gameObject);
    }

    protected override void OnInitiate() {
        gameObject.name = "InterScene";
        gamePlaySettings = Resources.Load<GamePlaySettings>("Settings/GamePlaySettings");
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene == SceneManager.GetSceneByName("GamePlayScene")) score = 0;

        var spawnManager = FindObjectOfType<SpawnManager>();
        var soundManager = FindObjectOfType<SoundManager>();
        var beatManager = FindObjectOfType<BeatManager>();

        if (spawnManager) {
            beatManager.bpm = level.bpm;
            spawnManager.spawnInfo = level.spawnInfo;
            if (level.song != null) soundManager.beatTest.clip = level.song.GenerateClip();

            beatManager.Initialise();
        }
    }
}
