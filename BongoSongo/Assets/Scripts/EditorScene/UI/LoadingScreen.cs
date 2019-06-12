using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour {

    public TMPro.TMP_Text text;

    void OnEnable () {
        StartCoroutine(TextCoroutine());
    }

    IEnumerator TextCoroutine () {
        var i = 0;

        while (true) {

            i++;

            if (i > 3) {
                i = 0;
            }

            text.text = "Loading" + new System.String('.', i);

            yield return new WaitForSeconds(0.25f);
        }
    }
}
