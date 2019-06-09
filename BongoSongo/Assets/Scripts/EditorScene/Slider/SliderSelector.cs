using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SliderSelector : Selector {

    public GameObject beatLengthTool;

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

    public override void OnToolsActive(RectTransform toolsUI) {
        var tool = Instantiate(beatLengthTool, toolsUI);

        tool.transform.SetSiblingIndex(0);

        toolsUI.GetComponent<SelectorTools>().extraTools.Add(tool);

        var inputField = tool.GetComponentInChildren<TMPro.TMP_InputField>();

        inputField.onEndEdit.AddListener(EnterValue);
        inputField.text = "" + GetComponentInParent<IndicatorInfo>().beatLenght;
    }

    public override void Delete() {
        Destroy(transform.parent.gameObject);
    }

    void EnterValue (string input) {
        try {
            GetComponentInParent<IndicatorInfo>().beatLenght = int.Parse(input);

            print("Set beat lenght to: " + GetComponentInParent<IndicatorInfo>().beatLenght);
        } catch (System.Exception e) {

        }
    }
}
