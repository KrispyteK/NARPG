using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePlaySettings : ScriptableObject {

    public float indicatorScale = 0.13f;

    public int sliderScore = 100;

    public int indicatorScoreOnTime = 100;
    public int indicatorScoreOnLate = 50;
    public float indicatorLateTime = 0.2f;
    public int indicatorBeatLife = 2;


#if UNITY_EDITOR
    [UnityEditor.MenuItem("Assets/Create/GamePlaySettings")]
    public static void CreateAsset() {
        var asset = ScriptableObject.CreateInstance<GamePlaySettings>();

        UnityEditor.AssetDatabase.CreateAsset(asset, "Assets/Resources/Settings/NewGamePlaySettings.asset");
        UnityEditor.AssetDatabase.SaveAssets();

        UnityEditor.EditorUtility.FocusProjectWindow();

        UnityEditor.Selection.activeObject = asset;
    }
#endif
}
