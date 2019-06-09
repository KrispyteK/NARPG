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

        Input.simulateMouseWithTouches = false;
    }

    void OnDestroy () {
        Input.simulateMouseWithTouches = true;
    }

    void StartTouch (Vector2 position) {
        if (EventSystem.current.IsPointerOverGameObject()) return;

        isDragging = false;
        canDrag = false;
        startedClickOnSelected = false;

        if (EditorManager.instance.selected && !selectedToolsUI.gameObject.activeSelf) {
            openToolsCoroutine = OpenTools();

            StartCoroutine(openToolsCoroutine);
        }

        beginMousePos = position;

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(position);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        var hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (selectDepth >= hits.Length) selectDepth = 0;

        if (!hits.Any()) return;

        var hit = hits[selectDepth];
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

    void StationaryTouch (Vector2 position) {
        if (!EditorManager.instance.selected) return;

        if ((beginMousePos - position).magnitude > pixelDragThreshold && !EventSystem.current.IsPointerOverGameObject() && !isDragging) {
            offset = EditorManager.instance.selected.transform.position - Camera.main.ScreenToWorldPoint(position);

            isDragging = true;

            StopOpenTools();
        }
    }

    void Drag (Vector2 position) {
        if (isDragging) {
            var targetPos = Camera.main.ScreenToWorldPoint(position) + offset;

            if (Input.GetKey(KeyCode.LeftControl)) {
                var normalized = targetPos / Camera.main.orthographicSize;

                normalized = new Vector3(Mathf.Round(normalized.x * 10) / 10, Mathf.Round(normalized.y * 10) / 10, 0);

                targetPos = normalized * Camera.main.orthographicSize;
            }

            EditorManager.instance.selected.transform.position = targetPos;
        }
    }

    void EndTouch () {
        StopOpenTools();

        if (EditorManager.instance.selected && !isDragging && !startedClickOnSelected) {
            if (selectedToolsUI.gameObject.activeSelf) {
                selectedToolsUI.gameObject.SetActive(false);
            } else {
                EditorManager.instance.Unselect();
            }
        }

        isDragging = false;
        canDrag = false;
    }

    void PhoneInput () {
        if (Input.touchCount > 0) {
            var touch = Input.GetTouch(0);

            switch (touch.phase) {
                case TouchPhase.Began:
                    StartTouch(touch.position);

                    break;
                case TouchPhase.Moved:
                case TouchPhase.Stationary:
                    StationaryTouch(touch.position);

                    Drag(touch.position);

                    break;
                case TouchPhase.Ended:
                    EndTouch();

                    break;
                case TouchPhase.Canceled:
                    Debug.LogWarning("Canceled");

                    break;
                default:
                    break;
            }
        }
    }

    void MouseInput () {
        if (Input.GetMouseButtonDown(0)) {
            StartTouch(Input.mousePosition);
        }

        if (EditorManager.instance.selected != null && Input.GetMouseButton(0)) {
            Vector2 mousePos = Input.mousePosition;

            StationaryTouch(mousePos);
        }

        if (Input.GetMouseButtonUp(0)) {
            EndTouch();
        }

        Drag(Input.mousePosition);
    }

    void Update() {
        if (Application.platform == RuntimePlatform.Android) {
            PhoneInput();
        } else {
            MouseInput();
        }
    }
}
