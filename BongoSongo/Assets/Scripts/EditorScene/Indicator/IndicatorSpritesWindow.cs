using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IndicatorSpritesWindow : MonoBehaviour {

    public GameObject spriteSelector;
    public RectTransform panel;


    void Start() {

        var indicatorSprites = Resources.Load<IndicatorSprites>("Settings/IndicatorSprites");

        for (int i = 0; i < indicatorSprites.sprites.Length; i++) {
            var instance = Instantiate(spriteSelector, panel);

            instance.GetComponent<Image>().sprite = indicatorSprites.sprites[i];
            instance.GetComponent<IndicatorSpriteSelector>().index = i;
            instance.GetComponent<IndicatorSpriteSelector>().window = gameObject.GetComponent<RectTransform>();
        }
    }

    
}
