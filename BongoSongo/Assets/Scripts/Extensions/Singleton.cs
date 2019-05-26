using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
    protected virtual bool shouldGenerateInstance => true;
    protected static T _instance;

    public static T Instance {
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

    public static T Initiate() {
        if (_instance != null) {
            return _instance;
        }

        var gameObject = new GameObject();
        var singleton = gameObject.AddComponent<T>();
        singleton.OnInitiate();

        DontDestroyOnLoad(gameObject);

        return singleton;
    }

    protected virtual void OnInitiate () {}
}