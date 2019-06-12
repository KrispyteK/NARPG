using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorSpritesWindow : MonoBehaviour {

    public GameObject spriteSelector;
    public RectTransform panel;

    private GameObject selectorToolsObject;

    void Update () {
        if (!selectorToolsObject.activeSelf) {
            Destroy(gameObject);
        }
    }

    void Start() {
        selectorToolsObject = FindObjectOfType<SelectorTools>().gameObject;

        var otherWindows = FindObjectsOfType<IndicatorSpritesWindow>();

        foreach (var otherWindow in otherWindows) {
            if (otherWindow != this) Destroy(otherWindow.gameObject); 
        }

        var indicatorSprites = Resources.Load<IndicatorSprites>("Settings/IndicatorSprites");

        for (int i = 0; i < indicatorSprites.sprites.Length; i++) {
            var instance = Instantiate(spriteSelector, panel);

            instance.GetComponent<Image>().sprite = indicatorSprites.sprites[i];
            instance.GetComponent<IndicatorSpriteSelector>().index = i;
            instance.GetComponent<IndicatorSpriteSelector>().window = gameObject.GetComponent<RectTransform>();
        }
    }

    
}
