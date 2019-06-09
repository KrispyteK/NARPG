using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SongScrollArrows : MonoBehaviour {

    public Image leftArrow;
    public Image rightArrow;

    private ScrollRectSnap scrollRectSnap;
    private ScrollRect scrollRect;

    private float smoothedAlphaLeft;
    private float smoothedAlphaRight;

    void Start() {
        scrollRectSnap = GetComponent<ScrollRectSnap>();
        scrollRect = GetComponent<ScrollRect>();

    }

    void Update() {
        //var alpha = Mathf.Clamp01(1 - Mathf.Abs(scrollRect.velocity.x / 100));

        var alpha = (scrollRectSnap.buttonDistance - Mathf.Abs(scrollRectSnap.DistanceToNearest) * 3) / scrollRectSnap.buttonDistance;

        smoothedAlphaLeft += (Mathf.Clamp01(alpha * (scrollRectSnap.index > 0 ? 1 : 0)) - smoothedAlphaLeft) * Time.deltaTime * 10f;
        smoothedAlphaRight += (Mathf.Clamp01(alpha * (scrollRectSnap.index < scrollRectSnap.buttons.Length - 1 ? 1 : 0)) - smoothedAlphaRight) * Time.deltaTime * 10f;

        leftArrow.color = Color.white * new Color(1,1,1, smoothedAlphaLeft);
        rightArrow.color = Color.white * new Color(1, 1, 1, smoothedAlphaRight);
    }
}
