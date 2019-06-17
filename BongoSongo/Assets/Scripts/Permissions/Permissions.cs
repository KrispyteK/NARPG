#if UNITY_ANDROID
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Permissions : MonoBehaviour {
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnBeforeSceneLoadRuntimeMethod() {
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera)) {
            Permission.RequestUserPermission(Permission.Camera);
        }

        if (!Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite)) {
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
        }
    }
}
#endif
