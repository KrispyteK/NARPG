using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderHandleSelector : Selector {

    public GameObject slider;

    private SliderHandles sliderHandles;
    private GameObject handlePrefab;

    private void Start () {
        sliderHandles = slider.GetComponent<SliderHandles>();

        handlePrefab = Resources.Load<GameObject>("Prefabs/SliderHandle");
    }

    public override void Select() {
        slider.GetComponent<Collider2D>().enabled = false;
    }

    public override void Unselect() {
        slider.GetComponent<Collider2D>().enabled = true;
    }

    public override void CreateNew() {
        var newHandle = Instantiate(handlePrefab, transform.parent);

        newHandle.transform.SetSiblingIndex(transform.GetSiblingIndex() + 1);
        newHandle.GetComponent<SliderHandleSelector>().slider = slider;
    }
    public override void Delete() {
        if (sliderHandles.handleTransform.childCount <= 2) return;

        Destroy(gameObject);
    }
}
