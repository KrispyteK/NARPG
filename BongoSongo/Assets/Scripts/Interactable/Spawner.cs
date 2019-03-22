using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public struct beatSpawn
{
	public int beat;
	public int bar;
}

public class Spawner : MonoBehaviour
{
	public GameObject hitThis;
	public beatSpawn[] whenToSpawn;

    // Start is called before the first frame update
    void Start()
    {
		
    }

	public void Spawn()
	{
		Instantiate (hitThis, transform.position, Quaternion.identity);
	}
}
