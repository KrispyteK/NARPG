using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorEditor : MonoBehaviour {

    Vector3 offset;
    
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null) {
                var target = hit.collider.gameObject;

                EditorManager.instance.selected = target;

                offset = target.transform.position - mousePos;
            } else {
                EditorManager.instance.selected = null;
            }
        }

        if (EditorManager.instance.selected != null && Input.GetMouseButton(0)) {
            var targetPos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;

            if (Input.GetKey(KeyCode.LeftControl)) {
                var normalized = targetPos / Camera.main.orthographicSize;

                normalized = new Vector3(Mathf.Round(normalized.x * 10) / 10, Mathf.Round(normalized.y * 10) / 10, 0);

                targetPos = normalized * Camera.main.orthographicSize;
            }

            EditorManager.instance.selected.transform.position = targetPos;
        }
    }
}
