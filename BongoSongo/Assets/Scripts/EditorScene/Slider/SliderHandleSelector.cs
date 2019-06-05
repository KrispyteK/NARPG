using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderHandleSelector : Selector {

    public GameObject slider;

    public override void Select() {
        slider.GetComponent<Collider2D>().enabled = false;
    }

    public override void Unselect() {
        slider.GetComponent<Collider2D>().enabled = true;
    }
}
