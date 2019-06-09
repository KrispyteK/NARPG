using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Linq;

public class IndicatorEditor : MonoBehaviour {

    public float selectedToolsTime = 1f;
    public int pixelDragThreshold = 2;
    public bool isDragging;
    public bool startedClickOnSelected;
    public RectTransform selectedToolsUI;

    Vector3 offset;
    int selectDepth = 0;
    Vector2 beginMousePos;
    bool canDrag;
    Selector lastSelected;
    IEnumerator openToolsCoroutine;
    SelectorTools selectorTools;

    IEnumerator OpenTools () {
        yield return new WaitForSeconds(selectedToolsTime);

        selectorTools.ResetTools();

        selectedToolsUI.gameObject.SetActive(true);

        if (EditorManager.instance.selected) EditorManager.instance.selected.OnToolsActive(selectedToolsUI);

        var position = (Vector2)Input.mousePosition - Camera.main.pixelRect.size / 2;
        position.x = Mathf.Min(position.x, Camera.main.pixelWidth/2 - selectedToolsUI.sizeDelta.x);
        position.y = Mathf.Min(position.y, Camera.main.pixelHeight/2 - selectedToolsUI.sizeDelta.y);

        selectedToolsUI.localPosition = position;

        startedClickOnSelected = true;
    }

    void StopOpenTools () {
        if (openToolsCoroutine != null) StopCoroutine(openToolsCoroutine);
    }

    void Start () {
        selectorTools = selectedToolsUI.GetComponent<SelectorTools>();
    }

    void Update() {

        if (Input.GetMouseButtonDown(0)) {
            if (EventSystem.current.IsPointerOverGameObject()) return;

            selectedToolsUI.gameObject.SetActive(false);

            isDragging = false;
            canDrag = false;
            startedClickOnSelected = false;

            openToolsCoroutine = OpenTools();

            StartCoroutine(openToolsCoroutine);

            beginMousePos = Input.mousePosition;

            Vector3 mousePos = Camera.main.ScreenToWorldPoint(beginMousePos);
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

                StopOpenTools();
            }
        }

        if (Input.GetMouseButtonUp(0)) {
            StopOpenTools();

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
