using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class EditorWindowInput : EditorWindow {

    protected bool isHoldingMouse;
    protected bool isHoldingShift;
    protected bool isHoldingCtrl;

    private Dictionary<KeyCode, bool> keyHeld = new Dictionary<KeyCode, bool>();
    private Dictionary<KeyCode, bool> keyOnHeld = new Dictionary<KeyCode, bool>();

    protected void CheckInput () {
        var e = Event.current;
        var controlID = GUIUtility.GetControlID(FocusType.Passive);

        switch (e.GetTypeForControl(controlID)) {
            case EventType.ScrollWheel:
                OnScroll(-(int)Mathf.Sign(e.delta.y));

                e.Use();
                break;
            case EventType.MouseUp:
                isHoldingMouse = false;

                OnMouseUp();
                OnMouseChanged();

                e.Use();
                break;
            case EventType.MouseDown:
                isHoldingMouse = true;

                OnMouseDown();
                OnMouseChanged();


                e.Use();
                break;
            case EventType.KeyDown:
                switch (e.keyCode) {
                    case KeyCode.LeftShift:
                        isHoldingShift = true;
                        break;
                    case KeyCode.LeftControl:
                        isHoldingCtrl = true;
                        break;
                }

                keyHeld[e.keyCode] = true;
                keyOnHeld[e.keyCode] = true;

                e.Use();
                break;

            case EventType.KeyUp:

                switch (e.keyCode) {
                    case KeyCode.LeftShift:
                        isHoldingShift = false;
                        break;
                    case KeyCode.LeftControl:
                        isHoldingCtrl = false;
                        break;
                }

                if (keyHeld.ContainsKey(e.keyCode)) keyHeld[e.keyCode] = false;
                if (keyOnHeld.ContainsKey(e.keyCode)) keyOnHeld[e.keyCode] = false;

                e.Use();
                break;
        }
    }

    protected void DrawQuad(Rect position, Color color) {
        Texture2D texture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
        texture.SetPixel(0, 0, color);
        texture.Apply();

        var old = GUI.skin.box.normal.background;

        GUI.skin.box.normal.background = texture;
        GUI.Box(position, GUIContent.none);

        GUI.skin.box.normal.background = old;
    }


    protected bool IsPressed(KeyCode keyCode) => keyHeld.ContainsKey(keyCode) && keyHeld[keyCode];
    protected bool OnPressed(KeyCode keyCode) {
        if (keyOnHeld.ContainsKey(keyCode) && keyOnHeld[keyCode]) {
            keyOnHeld[keyCode] = false;

            return true;
        }

        return false;
    }

    protected virtual void OnScroll (int delta) { }
    protected virtual void OnMouseDown () { }
    protected virtual void OnMouseUp() { }
    protected virtual void OnMouseChanged() { }
}
