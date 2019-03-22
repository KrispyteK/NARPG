using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

public struct beatSpawn
{
	public int beat;
	public int bar;

    public Vector2 position;
}

public class Spawner : MonoBehaviour
{
	public GameObject hitThis;
	public beatSpawn[] whenToSpawn;
    public RectTransform buttonCanvasParent;
    public GameObject visibleObject;

	public void Spawn(beatSpawn spawn)
	{
        var button = Instantiate (hitThis, CameraTransform.ScreenPointToWorld(spawn.position), Quaternion.identity);
        var interactable = button.GetComponent<Interactable>();

        //var visObj = Instantiate(visibleObject, buttonCanvasParent);

        //var visible = visObj.GetComponent<UIInteractionTest>();
        //visible.interactable = interactable;
    }
}
