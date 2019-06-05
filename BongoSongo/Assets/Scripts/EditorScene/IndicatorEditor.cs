using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class IndicatorEditor : MonoBehaviour {

    Vector3 offset;
    
    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            if (EventSystem.current.IsPointerOverGameObject()) {
                EditorManager.instance.Unselect();

                return;
            }

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            var hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);
            RaycastHit2D hit = hits.Any() ? hits.First() : default;

            var closestBeat = Mathf.Infinity;

            foreach (var h in hits) {
                if (!h.collider.gameObject.GetComponent<IndicatorInfo>()) continue;

                var dist = Mathf.Abs(EditorManager.instance.beat - h.collider.gameObject.GetComponent<IndicatorInfo>().beat);

                if (dist < closestBeat) {
                    closestBeat = dist;

                    hit = h;
                }
            }

            if (hit.collider != null) {
                EditorManager.instance.Unselect();

                var target = hit.collider.gameObject;

                EditorManager.instance.selected = target.GetComponent<Selector>();

                target.GetComponent<Selector>().Select();

                offset = target.transform.position - mousePos;
            } else if (EditorManager.instance.selected) {
                EditorManager.instance.Unselect();
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
