using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BeatScore : MonoBehaviour {

    private TextMeshPro textMesh;

    public void Init (int score) {
        textMesh = GetComponentInChildren<TextMeshPro>();
        textMesh.text = $"+{score}";

        StartCoroutine(Move());
    }

    IEnumerator Move () {
        var i = 0f;

        while (i < 1) {
            transform.position += Vector3.up * Time.deltaTime * 0.2f;

            textMesh.color = new Color(textMesh.color.r, textMesh.color.g, textMesh.color.b, 1 - i);

            i += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        Destroy(gameObject);
    }
}
