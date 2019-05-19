using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawerManager : MonoBehaviour {

    public static DrawerManager instance;

    private Drawer[] drawers;

    void Awake() {
        instance = this;

        drawers = FindObjectsOfType<Drawer>();
    }

    public void CloseAllBut(Drawer ignore) {
        foreach (var drawer in drawers) {
            if (drawer != ignore) {
                drawer.isOpened = false;
            }
        }
    }
}
