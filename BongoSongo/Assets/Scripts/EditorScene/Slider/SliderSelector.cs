using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderSelector : Selector {

    private SliderHandles sliderHandles;

    void Start() {
        sliderHandles = GetComponent<SliderHandles>();
    }

    public override void Select() {
        foreach (Transform handle in sliderHandles.handleTransform) {
            handle.GetComponent<Selector>().isSelectable = true;
        }
    }

    public override void Unselect() {
        foreach (Transform handle in sliderHandles.handleTransform) {
            handle.GetComponent<Selector>().isSelectable = false;
        }
    }
}
