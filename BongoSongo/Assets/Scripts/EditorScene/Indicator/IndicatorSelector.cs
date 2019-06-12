using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IndicatorSelector : Selector {

    public GameObject colorChangeTool;

    public override void OnToolsActive(RectTransform toolsUI) {
        var tool = Instantiate(colorChangeTool, toolsUI);

        tool.transform.SetSiblingIndex(0);

        toolsUI.GetComponent<SelectorTools>().extraTools.Add(tool);
    }
}
