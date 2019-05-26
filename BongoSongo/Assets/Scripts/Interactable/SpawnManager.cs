using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class SpawnManager : MonoBehaviour {
    public GameObject hitThis;
    public List<SpawnInfo> spawnInfo;

    public void Spawn(SpawnInfo spawn) {
        var position = new Vector3(spawn.position.x, spawn.position.y, 0) * Camera.main.orthographicSize;

        var button = Instantiate(hitThis, position + Vector3.forward * 5f, Quaternion.identity);
    }
}
