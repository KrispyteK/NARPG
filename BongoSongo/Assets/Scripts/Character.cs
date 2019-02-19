using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour {

    private Rigidbody2D rb;

    void Start()  {
        rb = GetComponent<Rigidbody2D>();

        MusicManager.Instance.OnBeat(() => {
            rb.AddForce(new Vector2(0, 100f * (MusicManager.Instance.MeasuredBass > 0.1f ? 1f : 0f)));
        });
    }

    void FixedUpdate() {
        //if (MusicManager.Instance.TimedBass > 0.25f) {
        //    rb.AddForce(new Vector2(0, 25f));
        //}
    }
}
