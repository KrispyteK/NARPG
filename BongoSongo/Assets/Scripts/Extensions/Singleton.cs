using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Singleton<T> {
    protected virtual bool DontDestroyOnLoad => false;
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

    private void CheckForMultiple () {
        var objs = FindObjectsOfType<T>();

        if (objs.Length > 1) {
            print($"Only one instance of type {this.GetType().Name} is allowed per scene.");
        }
    }

    public static T Initiate() {
        if (_instance != null) {
            return _instance;
        }

        var gameObject = new GameObject();
        var singleton = gameObject.AddComponent<T>();
        singleton.OnInitiate();

        if (singleton.DontDestroyOnLoad) DontDestroyOnLoad(gameObject);

        return singleton;
    }

    protected virtual void OnInitiate () {}
}