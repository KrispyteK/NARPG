using UnityEngine;

public class InterScene : MonoBehaviour {
    private static InterScene _instance;

    public Level level;
    public int score;
    public GamePlaySettings gamePlaySettings;

    public static InterScene Instance {
        get {
            if (_instance != null) {
                return _instance;
            }
            else {
                _instance = Initiate();

                return _instance;
            }
        }
    }

    public static InterScene Initiate() {
        if (_instance != null) {
            return _instance;
        }

        var gameObject = new GameObject("InterScene");
        var interScene = gameObject.AddComponent<InterScene>();

        interScene.gamePlaySettings = Resources.Load<GamePlaySettings>("Settings/GamePlaySettings");

        DontDestroyOnLoad(gameObject);

        return interScene;
    }

}
