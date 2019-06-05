using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderSelector : Selector {

    private SliderHandles sliderHandles;
    private Collider2D collider2D;


    void Start() {
        sliderHandles = GetComponent<SliderHandles>();
        collider2D = GetComponent<Collider2D>();
    }

    public override void Select() {
        foreach (Transform handle in sliderHandles.handleTransform) {
            handle.GetComponent<Selector>().isSelectable = true;
        }

        collider2D.enabled = false;
    }

    public override void Unselect() {
        foreach (Transform handle in sliderHandles.handleTransform) {
            handle.GetComponent<Selector>().isSelectable = false;
        }

        collider2D.enabled = true;
    }
}
