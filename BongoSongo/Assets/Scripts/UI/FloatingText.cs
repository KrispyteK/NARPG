using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingText : MonoBehaviour {
    private TextMeshPro textMesh;

    public static void Create (Vector2 location, string text, float time = 2f) {
        var resource = Resources.Load("Floating Text");

        var instance = Instantiate(resource, location, Quaternion.identity) as GameObject;

        instance.GetComponent<FloatingText>().Init(text,time);
    }

    public void Init(string text, float time = 2f) {
        textMesh = GetComponentInChildren<TextMeshPro>();
        textMesh.text = text;

        StartCoroutine(Move(time));
    }

    IEnumerator Move(float time) {
        var i = 0f;

        while (i < 1) {
            transform.position += Vector3.up * (Time.deltaTime / time);

            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 1 - i);

            i += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
