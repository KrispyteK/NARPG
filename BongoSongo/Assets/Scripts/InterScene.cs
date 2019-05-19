using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterScene : MonoBehaviour {
    private static InterScene _instance;

    public static InterScene Instance {
        get {
            if (_instance != null) {
                return _instance;
            } else {
                _instance = initiate();

                return _instance;
            }
        }
    }

    private static InterScene initiate () {
        var gameObject = new GameObject("InterScene");
        var interScene = gameObject.AddComponent<InterScene>();

        interScene.gamePlaySettings = Resources.Load<GamePlaySettings>("Settings/GamePlaySettings");

        DontDestroyOnLoad(gameObject);

        return interScene;
    }

    public int score;
    public GamePlaySettings gamePlaySettings;

    //void Awake () {
    //    if (instance == null) {
    //        instance = this;
    //    }
    //    else {
    //        Debug.LogError("Too many game managers in the scene!");
    //    }

    //    DontDestroyOnLoad(gameObject);
    //}
}
