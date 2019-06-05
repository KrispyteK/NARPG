using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class SpawnManager : MonoBehaviour {
    [System.Serializable]
    public struct IndicatorInfo {
        public Indicators indicator;
        public GameObject prefab;
    }

    public List<IndicatorInfo> editorPrefabs = new List<IndicatorInfo>();
    public List<SpawnInfo> spawnInfo;

    GameObject handlePrefab;

    private void Start () {
        handlePrefab = Resources.Load<GameObject>("Prefabs/SliderHandle");
    }

    public void Spawn(SpawnInfo spawn) {
        //var position = new Vector3(spawn.position.x, spawn.position.y, 0) * Camera.main.orthographicSize;

        //var button = Instantiate(hitThis, position + Vector3.forward * 5f, Quaternion.identity);

        var prefab = editorPrefabs.Find(x => x.indicator == spawn.indicator).prefab;
        var position = new Vector2(spawn.position.x, spawn.position.y) * Camera.main.orthographicSize;

        var instance = Instantiate(prefab, position, Quaternion.identity);
        //instance.GetComponent<IndicatorInfo>().beat = spawn.beat;
        //instance.GetComponent<IndicatorInfo>().spawnInfoIndex = spawn.beat;

        if (instance.CompareTag("Slider")) {
            var slider = instance.GetComponentInChildren<SliderIndicator>();
            var points = new List<Vector3>();

            foreach (var point in spawn.points) {
                //var newHandle = new GameObject();

                //newHandle.GetComponent<SliderHandleSelector>().slider = instance;
                //newHandle.transform.position = new Vector2(point.x, point.y) * Camera.main.orthographicSize;

                points.Add(new Vector3(point.x, point.y, 0) * Camera.main.orthographicSize);
            }

            slider.points = points.ToArray();
        }
    }

    //public void Load(string file) {
    //    foreach (Transform child in indicatorParent) {
    //        Destroy(child.gameObject);
    //    }

    //    level = Level.Load(file);

    //    levelInfo.SetInfo();
    //    LoadSong();

    //    var i = 0;

    //    var handlePrefab = Resources.Load<GameObject>("Prefabs/SliderHandle");

    //    foreach (var spawnInfo in level.spawnInfo) {
    //        var prefab = editorPrefabs.Find(x => x.indicator == spawnInfo.indicator).prefab;
    //        var position = new Vector2(spawnInfo.position.x, spawnInfo.position.y) * Camera.main.orthographicSize;

    //        var instance = Instantiate(prefab, position, Quaternion.identity, indicatorParent);
    //        instance.GetComponent<IndicatorInfo>().beat = spawnInfo.beat;
    //        instance.GetComponent<IndicatorInfo>().spawnInfoIndex = spawnInfo.beat;

    //        if (instance.CompareTag("SliderEditor")) {
    //            var sliderHandles = instance.GetComponentInChildren<SliderHandles>();

    //            foreach (Transform child in sliderHandles.handleTransform) Destroy(child.gameObject);

    //            foreach (var point in spawnInfo.points) {
    //                var newHandle = Instantiate(handlePrefab, sliderHandles.handleTransform);

    //                newHandle.GetComponent<SliderHandleSelector>().slider = instance;
    //                newHandle.transform.position = new Vector2(point.x, point.y) * Camera.main.orthographicSize;
    //            }
    //        }

    //        i = 0;
    //    }

    //    OrderIndicators();
    //}
}
