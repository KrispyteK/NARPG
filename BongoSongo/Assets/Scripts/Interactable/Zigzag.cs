using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zigzag : MonoBehaviour
{
    public float speed = 0.5f;
    public Vector2 start;
    public Vector2 end;

    private float i;
    private float d = 1;

    void Update() {
        i += speed * Time.deltaTime * d;

        if (i >= 1) d = -1;
        if (i <= 0) d = 1;

        var lerp = Vector2.Lerp(start, end, i);

        transform.position = Camera.main.ScreenToWorldPoint(new Vector3(
                lerp.x * Camera.main.pixelWidth,
                lerp.y * Camera.main.aspect * Camera.main.pixelHeight,
                -Camera.main.transform.position.z
            ));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(new Vector3(
                start.x * Camera.main.pixelWidth,
                start.y * Camera.main.aspect * Camera.main.pixelHeight,
                -Camera.main.transform.position.z
            )), 0.1f);

        Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(new Vector3(
                end.x * Camera.main.pixelWidth,
                end.y * Camera.main.aspect * Camera.main.pixelHeight,
                -Camera.main.transform.position.z
            )), 0.1f);
    }
}
