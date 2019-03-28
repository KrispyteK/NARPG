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

        transform.position = CameraTransform.ScreenPointToWorld(new Vector2(
                lerp.x,
                lerp.y
            ));
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawSphere(CameraTransform.ScreenPointToWorld(new Vector2(
                start.x,
                start.y
            )), 0.1f);

        Gizmos.DrawSphere(CameraTransform.ScreenPointToWorld(new Vector2(
                end.x,
                end.y
            )), 0.1f);
    }
}
