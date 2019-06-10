using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
    protected virtual bool DontDestroyLoad => false;
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

    private static bool CheckForMultiple () {
        var objs = FindObjectsOfType<T>();
        var hasMultiple = objs.Length > 1;

        if (hasMultiple) {
            print($"Only one instance of type {typeof(T)} is allowed per scene.");
        }

        return hasMultiple;
    }

    public static T Initiate() {
        if (CheckForMultiple()) return _instance;

        if (_instance != null) {
            return _instance;
        }


        var gameObject = new GameObject();
        var singleton = gameObject.AddComponent<T>();
        singleton.OnInitiate();

        if (singleton.DontDestroyLoad) DontDestroyOnLoad(gameObject);

        return singleton;
    }

    protected virtual void OnInitiate () {}
}