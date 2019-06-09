using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class IndicatorEditor : MonoBehaviour {

    public int pixelDragThreshold = 2;
    public bool isDragging;
    public bool startedClickOnSelected;

    Vector3 offset;
    int selectDepth = 0;
    Vector2 beginMousePos;
    bool canDrag;
    Selector lastSelected;

    void Update() {

        if (Input.GetMouseButtonDown(0)) {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            isDragging = false;
            canDrag = false;

            beginMousePos = Input.mousePosition;

            startedClickOnSelected = false;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            var hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);
            RaycastHit2D hit = hits.Any() ? hits.First() : default;

            if (selectDepth >= hits.Length) selectDepth = 0;

            if (!hits.Any()) return;

            hit = hits[selectDepth];
            var target = hit.collider.gameObject;
            var selector = target.GetComponent<Selector>();

            if (hit.collider != null) {
                EditorManager.instance.Unselect();

                selector.Select();

                lastSelected = selector;

                EditorManager.instance.selected = target.GetComponent<Selector>();

                startedClickOnSelected = lastSelected == EditorManager.instance.selected;

                selectDepth++;
            }
        }

        if (EditorManager.instance.selected != null && Input.GetMouseButton(0)) {
            Vector2 mousePos = Input.mousePosition;

            if ((beginMousePos - mousePos).magnitude > pixelDragThreshold && !EventSystem.current.IsPointerOverGameObject() && !isDragging) {
                offset = EditorManager.instance.selected.transform.position - Camera.main.ScreenToWorldPoint(mousePos);

                isDragging = true;
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            if (EditorManager.instance.selected && !isDragging && !startedClickOnSelected) {
                EditorManager.instance.Unselect();
            }

            isDragging = false;
            canDrag = false;
        }

        if (isDragging) {
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
